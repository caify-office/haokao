namespace HaoKao.BasicService.Application.ViewModels.Account;

public class ChangePhoneCodeViewModel
{
    private string _oldPhone;
    /// <summary>
    /// 原手机号码
    /// </summary>
    [DisplayName("原手机号码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string OldPhone
    {
        get => _oldPhone.Base64Decode();
        set => _oldPhone = value;
    }

    private string _oldPhoneCode;
    /// <summary>
    /// 原手机号码验证码
    /// </summary>
    [DisplayName("原手机号码验证码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string OldPhoneCode
    {
        get => _oldPhoneCode.Base64Decode();
        set => _oldPhoneCode = value;
    }
    
    private string _newPhone;
    /// <summary>
    /// 新手机号码
    /// </summary>
    [DisplayName("新手机号码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string NewPhone
    {
        get => _newPhone.Base64Decode();
        set => _newPhone = value;
    }

    private string _newPhoneCode;
    /// <summary>
    /// 新手机号码验证码
    /// </summary>
    [DisplayName("新手机号码验证码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string NewPhoneCode
    {
        get => _newPhoneCode.Base64Decode();
        set => _newPhoneCode = value;
    }
}