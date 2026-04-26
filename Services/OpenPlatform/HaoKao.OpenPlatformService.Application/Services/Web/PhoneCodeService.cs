using Girvs.EventBus;
using HaoKao.Common;
using HaoKao.Common.Events.NotificationMessage;
using HaoKao.OpenPlatformService.Application.ViewModels.PhoneCode;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

/// <summary>
/// 用户端发送验证服务
/// </summary>
[AllowAnonymous]
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
public class PhoneCodeService(
    IStaticCacheManager cacheManager,
    IEventBus eventBus,
    IRandomVerificationCodeService randomVerificationCodeService)
    : IPhoneCodeService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    private readonly IRandomVerificationCodeService _randomVerificationCodeService = randomVerificationCodeService ??
                                                                                     throw new ArgumentNullException(nameof(randomVerificationCodeService));

    // 手机短信验证码万能Key
    private const string SecretCode = "zhuofan168";

    /// <summary>
    /// 设置过期时间为30分钟
    /// </summary>
    private static readonly int PhoneCodeExpiredTime = 30;

    /// <summary>
    /// 设置重复发送限制时间为300秒
    /// </summary>
    private static readonly int RepeatSendingLimitTime = 300;

    private CacheKey BuilderCodeSendCacheKey(EventNotificationMessageType messageType, string phone)
    {
        //设置过期时间为60秒
        return new CacheKey("{0}")
            .Create(
                messageType.ToString(),
                phone,
                PhoneCodeExpiredTime
            );
    }

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<bool> SendPhoneCode([FromBody] SendPhoneCodeViewModel model)
    {
        await _randomVerificationCodeService.VerificationCode(model.RandomMark, model.RandomVerificationCode);

        var cacheKey = BuilderCodeSendCacheKey(model.MessageType, model.Phone);

        var codeModel = _cacheManager.Get<CacheCodeModel>(cacheKey, () => null);

        if (codeModel != null && codeModel.SendTime.AddSeconds(RepeatSendingLimitTime) > DateTime.Now)
        {
            throw new GirvsException(
                $"{model.Phone}{CodeDesc.SendCodeDesc[model.MessageType]}验证码发送过于频繁，{RepeatSendingLimitTime}秒内请勿重复发送",
                568);
        }

#pragma warning disable CS0618 // 类型或成员已过时
        codeModel = new CacheCodeModel
        {
            SendTime = DateTime.Now,
            Phone = model.Phone,
            Code = CommonHelper.GenerateRandomInteger(100000, 999999).ToString()
        };
#pragma warning restore CS0618 // 类型或成员已过时

        var codeTextDesc = CodeDesc.SendCodeDesc[model.MessageType];
        var message = new SendMobileNotificationMessageEvent(
            codeTextDesc,
            model.MessageType,
            string.Empty,
            model.Phone,
            [codeModel.Code, "30",]
        );
        await _eventBus.PublishAsync(message);

       await _cacheManager.SetAsync(cacheKey, codeModel);
        return true;
    }

    [NonAction]
    public bool CheckPhoneCode(EventNotificationMessageType messageType, string phone, string code)
    {
        if (code == SecretCode) return true;

        var cacheKey = BuilderCodeSendCacheKey(messageType, phone);
        var codeModel = _cacheManager.Get<CacheCodeModel>(cacheKey, () => null);

        var codeTextDesc = CodeDesc.SendCodeDesc[messageType];

        if (codeModel == null || codeModel.Code != code) throw new GirvsException($"输入的{codeTextDesc}错误或者已过期", 568);

        return true;
    }
}

public static class CodeDesc
{
    public static Dictionary<EventNotificationMessageType, string> SendCodeDesc =
        new()
        {
            { EventNotificationMessageType.Login, "登录验证码" },
            { EventNotificationMessageType.Register, "注册验证码" },
            { EventNotificationMessageType.RetrievePassword, "找回密码验证码" },
            { EventNotificationMessageType.ChangePhoneNumber, "更改绑定手机验证码" }
        };
}