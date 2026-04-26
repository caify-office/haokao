using Girvs.Extensions;
using HaoKao.Common.Events.NotificationMessage;

namespace HaoKao.OpenPlatformService.Application.ViewModels.PhoneCode;

public class SendPhoneCodeViewModel
{
    /// <summary>
    /// 手机号码
    /// </summary>
    [DisplayName("手机号码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Phone
    {
        get => _phone.Base64Decode();
        set => _phone = value;
    }
    private string _phone;

    [DisplayName("验证码类型")]
    public EventNotificationMessageType MessageType { get; set; }

    /// <summary>
    /// 获取验证码的随机标识
    /// </summary>
    [DisplayName("验证码的随机标识")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string RandomMark
    {
        get => _randomMark.Base64Decode();
        set => _randomMark = value;
    }
    private string _randomMark;

    /// <summary>
    /// 随机验证码
    /// </summary>
    [DisplayName("随机验证码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string RandomVerificationCode
    {
        get => _randomVerificationCode.Base64Decode();
        set => _randomVerificationCode = value;
    }
    private string _randomVerificationCode;
}