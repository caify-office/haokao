using HaoKao.BasicService.Application.Interfaces;
using HaoKao.BasicService.Domain.Entities;
using HaoKao.Common;
using HaoKao.Common.Events.NotificationMessage;

namespace HaoKao.BasicService.Application.Services;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class AccountService(
    IUserRepository userRepository,
    IRandomVerificationCodeService randomVerificationCodeService,
    IStaticCacheManager cacheManager,
    IEventBus eventBus,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications
) : IAccountService
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IRandomVerificationCodeService _randomVerificationCodeService = randomVerificationCodeService ?? throw new ArgumentNullException(nameof(randomVerificationCodeService));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

    // 手机短信验证码万能Key
    private const string _SecretCode = "zhuofan168";

    /// <summary>
    /// 通过登陆名称，获取相关的考试列表
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    [HttpGet("{account}")]
    [AllowAnonymous]
    public async Task<List<TenantEntityViewModel>> GetTenants(string account)
    {
        var users = await _userRepository.GetAllUserAsync(account);
        return users.OrderByDescending(x => x.CreateTime)
                    .Select(x => new TenantEntityViewModel { TenantId = x.TenantId, TenantName = x.TenantName })
                    .ToList();
    }

    /// <summary>
    /// 登陆第一步验证，用户名和密码
    /// </summary>
    /// <param name="model">登陆模型</param>
    /// <returns>返回手机号码后4位</returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<UserLoginStepOneReturnViewModel> UserLoginStepOne([FromBody] UserLoginStepOneViewModel model)
    {
        CheckPasswordErrorCount(model.UserAccount);

        var user = await _userRepository.GetUserByLoginNameAndTenantIdAsync(model.UserAccount, model.TenantId);

        if (user == null || user.UserPassword != model.Password.ToMd5())
        {
            WritePasswordErrorCount(model.UserAccount);
            throw new GirvsException("用户名或者密码错误，请重新输入");
        }

        if (user.State == DataState.Disable)
        {
            throw new GirvsException("当前用户已被禁用，无法登陆");
        }

        ClearPasswordErrorCount(model.UserAccount);

        return new UserLoginStepOneReturnViewModel
        {
            Phone = user.ContactNumber.IsNullOrWhiteSpace() ? string.Empty : $"*******{user.ContactNumber[7..]}",
            ValidString = $"{_SecretCode}{user.ContactNumber}".ToMd5()
        };
    }

    private string BuilderTokenString(User user, UserLoginSecondStepViewModel model)
    {
        var girvsIdentityClaim = new GirvsIdentityClaim
        {
            UserId = user.Id.ToString(),
            UserName = user.UserName ?? user.UserAccount,
            TenantId = user.TenantId.ToString(),
            TenantName = user.TenantName,
            IdentityType = IdentityType.ManagerUser,
            SystemModule = model.SystemModule,
            OtherClaims = new Dictionary<string, string>
            {
                { GirvsClaimManagerExtensions.GirvsIdentityUserTypeClaimTypes, user.UserType.ToString() },
                { "ExamBehavior", model.ExamBehavior }
            }
        };

        return JwtBearerAuthenticationExtension.GenerateToken(girvsIdentityClaim);
    }

    /// <summary>
    /// 登陆第二步验证，用户名和密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<UserLoginDetailViewModel> UserLoginStepSecond([FromBody] UserLoginSecondStepViewModel model)
    {
        CheckPhoneCode(EventNotificationMessageType.Login, model.Phone, model.PhoneCode);

        var user = await _userRepository.GetUserByLoginNameAndTenantIdAsync(model.UserAccount, model.TenantId);
        if (user == null || user.UserPassword != model.Password.ToMd5())
        {
            throw new GirvsException("用户名或者密码错误，请重新输入");
        }

        if (user.State == DataState.Disable)
        {
            throw new GirvsException("当前用户已被禁用，无法登陆");
        }

        var result = user.MapToDto<UserLoginDetailViewModel>();
        result.EnforceChangePassword = model.Password == "123456";
        result.ContactNumber = model.Phone;

        if (!string.IsNullOrWhiteSpace(user.ContactNumber) && user.ContactNumber != model.Phone)
        {
            if (model.PhoneCode != _SecretCode)
            {
                throw new GirvsException("当前用户手机号错误，请重新输入");
            }
        }

        // 如果该用户没有绑定手机号，则绑定获取验证码手机号
        if (string.IsNullOrWhiteSpace(user.ContactNumber) && model.PhoneCode != _SecretCode)
        {
            //初始化请求头
            var claims = new Dictionary<string, string>
            {
                { GirvsIdentityClaimTypes.UserId, user.Id.ToString() },
                { GirvsIdentityClaimTypes.UserName, user.UserName },
                { GirvsIdentityClaimTypes.TenantId, user.TenantId.ToString() },
                { GirvsIdentityClaimTypes.TenantName, user.TenantName },
                { GirvsIdentityClaimTypes.IdentityType, nameof(IdentityType.ManagerUser) },
                { GirvsIdentityClaimTypes.ClaimSystemModule, model.SystemModule.ToString() }
            };
            EngineContext.Current.ClaimManager.SetFromDictionary(claims);

            var buildContactNumberCommand = new BindContactNumberCommand(user.Id, model.Phone);
            await _bus.SendCommand(buildContactNumberCommand);
            if (_notifications.HasNotifications())
            {
                var errorMessage = _notifications.GetNotificationMessage();
                throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
            }
        }

        result.Token = BuilderTokenString(user, model);
        return result;
    }

    /// <summary>
    /// 考试管理员登录进去后输入目标考试管理员账号和密码信息获取对应考试新的token
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<UserLoginDetailViewModel> ChangeTenant([FromBody] ChangeExamViewModel model)
    {
        var user = await _userRepository.GetUserByLoginNameAndTenantIdAsync(model.UserAccount, model.TenantId);
        if (user == null || user.UserPassword != model.Password.ToMd5())
        {
            throw new GirvsException("密码错误，请重新输入");
        }

        if (user.State == DataState.Disable)
        {
            throw new GirvsException("当前用户已被禁用，无法登陆");
        }

        var result = user.MapToDto<UserLoginDetailViewModel>();
        result.EnforceChangePassword = model.Password == "123456";
        var modelToken = new UserLoginSecondStepViewModel
        {
            ExamBehavior = model.ExamBehavior.Base64Encode(),
            SystemModule = model.SystemModule,
        };
        result.Token = BuilderTokenString(user, modelToken);
        return result;
    }

    /// <summary>
    /// 更换绑定手机号码发送验证码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> SendChangePhoneCode(SendChangePhoneCodeViewModel model)
    {
        return await SendPhoneCode(EventNotificationMessageType.ChangePhoneNumber, model.Phone, model.RandomMark, model.RandomVerificationCode);
    }

    /// <summary>
    /// 更换手机号码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> ChangePhoneCode(ChangePhoneCodeViewModel model)
    {
        CheckPhoneCode(EventNotificationMessageType.ChangePhoneNumber, model.OldPhone, model.OldPhoneCode);
        CheckPhoneCode(EventNotificationMessageType.ChangePhoneNumber, model.NewPhone, model.NewPhoneCode);
        var currentUserId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var command = new BindContactNumberCommand(currentUserId, model.NewPhone);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    #region 发送验证码和较验验证码

    /// <summary>
    /// 设置过期时间为30分钟
    /// </summary>
    private const int _PhoneCodeExpiredTime = 30;

    /// <summary>
    /// 设置重复发送限制时间为120秒
    /// </summary>
    private const int _RepeatSendingLimitTime = 120;

    private static CacheKey BuilderCodeSendCacheKey(EventNotificationMessageType messageType, string phone)
    {
        //设置过期时间为60秒
        return new CacheKey("{0}").Create(messageType.ToString(), phone, _PhoneCodeExpiredTime);
    }

    /// <summary>
    /// 较验验证码
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    private bool CheckPhoneCode(EventNotificationMessageType messageType, string phone, string code)
    {
        if (code == _SecretCode) return true;

        var cacheKey = BuilderCodeSendCacheKey(messageType, phone);
        var codeModel = _cacheManager.Get<CacheCodeModel>(cacheKey, () => null);

        var codeTextDesc = _sendCodeDesc[messageType];
        if (codeModel == null || codeModel.Code != code) throw new GirvsException($"输入的{codeTextDesc}错误或者已过期");

        return true;
    }

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [AllowAnonymous]
    public async Task<bool> SendPhoneCode([FromBody] SendPhoneCodeViewModel model)
    {
        var emptyPhoneValidCode = $"{_SecretCode}".ToMd5();
        var phoneValidCode = $"{_SecretCode}{model.Phone}".ToMd5();

        if (model.PhoneSecurityCode != phoneValidCode && emptyPhoneValidCode != model.PhoneSecurityCode)
        {
            throw new GirvsException("当前用户手机号错误，请重新输入");
        }

        return await SendPhoneCode(EventNotificationMessageType.Login, model.Phone, model.RandomMark, model.RandomVerificationCode);
    }

    private readonly Dictionary<EventNotificationMessageType, string> _sendCodeDesc = new()
    {
        { EventNotificationMessageType.Login, "登录验证码" },
        { EventNotificationMessageType.Register, "注册验证码" },
        { EventNotificationMessageType.RetrievePassword, "找回密码验证码" },
        { EventNotificationMessageType.ChangePhoneNumber, "更改绑定手机验证码" },
    };

    private async Task<bool> SendPhoneCode(EventNotificationMessageType messageType, string phone, string randomMark, string randomVerificationCode)
    {
        await _randomVerificationCodeService.VerificationCode(randomMark, randomVerificationCode);

        var cacheKey = BuilderCodeSendCacheKey(messageType, phone);

        var codeModel = _cacheManager.Get<CacheCodeModel>(cacheKey, () => null);

        var codeTextDesc = _sendCodeDesc[messageType];
        if (codeModel != null && codeModel.SendTime.AddSeconds(_RepeatSendingLimitTime) > DateTime.Now)
        {
            throw new GirvsException($"{phone}{codeTextDesc}发送过于频繁，请于2分钟后再发送");
        }
        codeModel = new CacheCodeModel
        {
            SendTime = DateTime.Now,
            Phone = phone,
            Code = _randomVerificationCodeService.GetRandomNumber(100000, 999999),
        };

        var message = new SendMobileNotificationMessageEvent(codeTextDesc, messageType, string.Empty, phone, [codeModel.Code, "30",]);

        await _eventBus.PublishAsync(message);

        await _cacheManager.SetAsync(cacheKey, codeModel);

        return true;
    }

    #endregion

    #region 密码输入次数达5次，则进行锁定

    private const int _MaxErrorNumber = 5;

    CacheKey BuilderPasswordErrorCountCacheKey(string account)
    {
        return new CacheKey("BasicService:{0}")
            .Create(
                account,
                "CountPasswordError",
                10
            );
    }

    void ClearPasswordErrorCount(string account)
    {
        var cacheKey = BuilderPasswordErrorCountCacheKey(account);
        _cacheManager.RemoveAsync(cacheKey).Wait();
    }

    bool CheckPasswordErrorCount(string account)
    {
        var cacheKey = BuilderPasswordErrorCountCacheKey(account);
        var count = _cacheManager.Get(cacheKey, () => "0");
        if (int.Parse(count) >= _MaxErrorNumber)
        {
            throw new GirvsException("由于密码连续输错5次，当前账号已被锁定，请10分钟后进行重试");
        }
        return true;
    }

    void WritePasswordErrorCount(string account)
    {
        var cacheKey = BuilderPasswordErrorCountCacheKey(account);
        var count = _cacheManager.Get(cacheKey, () => "0");
        _cacheManager.SetAsync(cacheKey, (int.Parse(count) + 1).ToString()).Wait();
    }

    #endregion
}