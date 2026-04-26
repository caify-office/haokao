using Girvs.Extensions;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

/// <summary>
/// 更换绑定手机号码
/// </summary>
public class ChangePhoneViewModel
{
    private string _oldPhone;

    /// <summary>
    /// 旧的手机号码 BASE64加密
    /// </summary>
    public string OldPhone
    {
        get => _oldPhone.Base64Decode();
        set => _oldPhone = value;
    }

    private string _newPhone;

    /// <summary>
    /// 新的手机号码 BASE64加密
    /// </summary>
    public string NewPhone
    {
        get => _newPhone.Base64Decode();
        set => _newPhone = value;
    }

    private string _newPhoneCode;

    /// <summary>
    /// 新的手机号码验证码 BASE64加密
    /// </summary>
    public string NewPhoneCode
    {
        get => _newPhoneCode.Base64Decode();
        set => _newPhoneCode = value;
    }
}