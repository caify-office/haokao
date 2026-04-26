using Girvs.Extensions;
using HaoKao.Common.Events.NotificationMessage;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;
using HaoKao.OpenPlatformService.Application.ViewModels.Account;
using HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;
using HaoKao.OpenPlatformService.Domain.Commands.UserDailyActivityLog;
using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Repositories;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public class LoginHelpService(
    IRegisterUserRepository registerUserRepository,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IPhoneCodeService phoneCodeService,
    IExternalIdentityService externalIdentityService,
    IStaticCacheManager staticCacheManager
) : ILoginHelpService
{
    private readonly IRegisterUserRepository _registerUserRepository = registerUserRepository ?? throw new ArgumentNullException(nameof(registerUserRepository));
    private readonly IPhoneCodeService _phoneCodeService = phoneCodeService ?? throw new ArgumentNullException(nameof(phoneCodeService));
    private readonly IExternalIdentityService _externalIdentityService = externalIdentityService ?? throw new ArgumentNullException(nameof(externalIdentityService));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    public async Task<RegisterUser> LoginRegisterUser(LoginInput loginInput)
    {
        RegisterUser user;
        switch (loginInput.LoginType)
        {
            case LoginType.LocalPhoneLogin:
                loginInput.PasswordCode = "zhuofan168";
                user = await PhoneCodeLogin(loginInput);
                break;

            case LoginType.PhonePasswordLogin:
                user = await PhonePasswordLogin(loginInput);
                break;

            case LoginType.PhoneCodeLogin:
                user = await PhoneCodeLogin(loginInput);
                break;

            case LoginType.ExternalIdentityLogin:
                user = await ExternalIdentityLogin(loginInput);
                break;

            case LoginType.ExternalIdentityPhoneCodeLogin:
                user = await ExternalIdentityPhoneCodeLogin(loginInput);
                break;

            default: throw new ArgumentOutOfRangeException();
        }

        if (user != null)
        {
            var command = new UpdateLastLoginCommand(user.Id);
            await _bus.SendCommand(command);

            // 添加用户每日活跃记录
            var createUserDailyActivityLogCommand = new CreateDailyActiveUserLogCommand(user.Id, loginInput.ClientId);
            await _bus.SendCommand(createUserDailyActivityLogCommand);

            VerifyCommand();
            return user;
        }

        throw new GirvsException(StatusCodes.Status400BadRequest, "登陆错误，相关的用户参数传递错误");
    }

    /// <summary>
    /// 手机号码加密码登录
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    private async Task<RegisterUser> PhonePasswordLogin(LoginInput loginInput)
    {
        var user = await GetRegisterUserByPhone(loginInput.Phone);

        if (user == null || user.Password != loginInput.PasswordCode.ToMd5())
        {
            throw new GirvsException("用户名或密码错误");
        }

        return user;
    }

    /// <summary>
    /// 手机号加短信验证登录或注册登录
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    private async Task<RegisterUser> PhoneCodeLogin(LoginInput loginInput)
    {
        //验证手机短信验证码
        CheckPhoneCode(EventNotificationMessageType.Login, loginInput.Phone, loginInput.PasswordCode);

        var user = await GetRegisterUserByPhone(loginInput.Phone);
        if (user != null)
        {
            if (!string.IsNullOrEmpty(loginInput.UnionId))
            {
                var externalIdentity = user.ExternalIdentities.FirstOrDefault(x => x.Scheme == "Weixin");

                // 判断是否绑定了外部用户
                if (externalIdentity == null)
                {
                    // 绑定微信
                    var bindExternalUserCommand = new BindExternalUserCommand(loginInput.Phone,
                        new ExternalUserCommand("Weixin", loginInput.UnionId, null, 0, null, null, []));
                    await _bus.SendCommand(bindExternalUserCommand);
                    VerifyCommand();
                }

                // 如果当前用户已经绑定了微信用户，但是当前登录的微信用户和绑定的微信用户不一致
                else if (externalIdentity.UniqueIdentifier != loginInput.UnionId)
                {
                    throw new GirvsException("当前手机号码已绑定了其他微信用户");
                }
            }
            return user;
        }

        var externalUserCommand = string.IsNullOrEmpty(loginInput.UnionId)
            ? null : new ExternalUserCommand("Weixin", loginInput.UnionId, null, 0, null, null, []);
        var command = new CreateRegisterUserCommand(loginInput.Phone, loginInput.ClientId, externalUserCommand);
        await _bus.SendCommand(command);
        VerifyCommand();
        return await GetRegisterUserByPhone(loginInput.Phone);
    }

    /// <summary>
    /// 外部认证登录，提前绑定了用户注册
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    private async Task<RegisterUser> ExternalIdentityLogin(LoginInput loginInput)
    {
        var (scheme, externalUser) = await GetExternalUserInfo(loginInput);

        var externalIdentity =
            await _registerUserRepository.GetByExternalIdentity(scheme, externalUser.UniqueIdentifier);

        if (externalIdentity?.RegisterUser == null)
        {
            throw new GirvsException(StatusCodes.Status400BadRequest, "登陆错误，未找当前绑定的用户");
        }

        return externalIdentity.RegisterUser;
    }

    /// <summary>
    /// 外部认证后，注册绑定用户手机号并登录
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    private async Task<RegisterUser> ExternalIdentityPhoneCodeLogin(LoginInput loginInput)
    {
        //校验短信验证码
        CheckPhoneCode(EventNotificationMessageType.Login, loginInput.Phone, loginInput.PasswordCode);

        var (scheme, externalUser) = await GetExternalUserInfo(loginInput);

        if (scheme.IsNullOrEmpty() || externalUser == null || externalUser.UniqueIdentifier.IsNullOrEmpty())
        {
            throw new GirvsException("未读取到对应的外部登录用户");
        }

        var externalUserCommand = new ExternalUserCommand(
            externalUser.Schemem,
            externalUser.UniqueIdentifier,
            externalUser.NikeName,
            externalUser.UserGender,
            externalUser.EmailAddress,
            externalUser.HeadImage,
            externalUser.OtherInformation
        );

        var user = await GetRegisterUserByPhone(loginInput.Phone);
        if (user == null)
        {
            //如果手机号码对应的用户不存在，则直接进行创建并绑定当前的外部用户
            var command = new CreateRegisterUserCommand(loginInput.Phone, loginInput.ClientId, externalUserCommand);
            await _bus.SendCommand(command);
        }
        else
        {
            //如果对应的用户存在
            if (user.ExternalIdentities.Any(x => x.Scheme == scheme))
            {
                throw new GirvsException($"当前手机号码用户已绑定了'{scheme}'用户");
            }

            var command = new BindExternalUserCommand(loginInput.Phone, externalUserCommand);
            await _bus.SendCommand(command);
        }

        VerifyCommand();

        return await GetRegisterUserByPhone(loginInput.Phone);
    }

    /// <summary>
    /// 获取外部用户
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    private async Task<(string, ExternalUser)> GetExternalUserInfo(LoginInput loginInput)
    {
        var (scheme, externalUser) = await _externalIdentityService.GetExternalUserInfo();
        return (scheme, externalUser);
        // if (loginInput.ExternalUser == null || loginInput.ExternalUser.Scheme.IsNullOrEmpty() ||
        //     loginInput.ExternalUser.Unionid.IsNullOrEmpty() || loginInput.ExternalUser.Nickname.IsNullOrEmpty())
        // {
        //     throw new GirvsException(StatusCodes.Status400BadRequest, "登陆错误，相关的用户参数传递错误");
        // }
        //
        // return (loginInput.ExternalUser.Scheme, new ExternalUser
        // {
        //     Schemem = loginInput.ExternalUser.Scheme,
        //     HeadImage = loginInput.ExternalUser.Headimgurl,
        //     NikeName = loginInput.ExternalUser.Nickname,
        //     UniqueIdentifier = loginInput.ExternalUser.Unionid,
        //     OtherInformation = loginInput.ExternalUser.BuildeOtherInformation()
        // });
    }

    /// <summary>
    /// 指定用户当前是否绑定了对应的外部用户
    /// </summary>
    /// <param name="scheme">外部用户认证方式</param>
    /// <param name="phone">手机号码</param>
    /// <returns></returns>
    private async Task<bool> CurrentUserIsBindExternalUser(string scheme, string phone)
    {
        var user = await GetRegisterUserByPhone(phone);
        return user.ExternalIdentities.Any(x => x.Scheme == scheme);
    }

    /// <summary>
    /// 根据手机号码获取注册用户
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    private async Task<RegisterUser> GetRegisterUserByPhone(string phone)
    {
        var phoneKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(phone);
        return await _staticCacheManager.GetAsync(phoneKey, () => _registerUserRepository.GetByInclude(x => x.Phone == phone));
    }

    /// <summary>
    /// 较验事件相关的执行情况
    /// </summary>
    /// <exception cref="GirvsException"></exception>
    private void VerifyCommand()
    {
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 校验手机号码和短信验证码
    /// </summary>
    /// <param name="eventNotificationMessageType"></param>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    private bool CheckPhoneCode(EventNotificationMessageType eventNotificationMessageType, string phone, string code)
    {
        return _phoneCodeService.CheckPhoneCode(eventNotificationMessageType, phone, code);
    }
}