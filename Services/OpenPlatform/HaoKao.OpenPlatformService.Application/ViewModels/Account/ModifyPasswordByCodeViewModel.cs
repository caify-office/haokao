using Girvs.Extensions;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

public class ModifyPasswordByCodeViewModel
{
    private string _phone;

    /// <summary>
    /// 新的手机号码 Base64加密
    /// </summary>
    public string Phone
    {
        get => _phone.Base64Decode();
        set => _phone = value;
    }

    private string _phoneCode;

    /// <summary>
    /// 新的手机号码验证码 Base64加密
    /// </summary>
    public string PhoneCode
    {
        get => _phoneCode.Base64Decode();
        set => _phoneCode = value;
    }
    
    private string _newPassword;

    /// <summary>
    ///  新的密码 Base64加密
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    public string NewPassword
    {
        get => _newPassword?.Base64Decode();
        set => _newPassword = value;
    }
}