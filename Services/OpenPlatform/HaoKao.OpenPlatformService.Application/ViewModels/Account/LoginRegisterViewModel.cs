using Girvs.Extensions;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

/// <summary>
/// 用户登陆注册
/// </summary>
public class LoginRegisterViewModel
{
    private string _phone;

    /// <summary>
    /// 新的手机号码
    /// </summary>
    public string Phone
    {
        get => _phone.Base64Decode();
        set => _phone = value;
    }

    private string _phoneCode;

    /// <summary>
    /// 新的手机号码验证码
    /// </summary>
    public string PhoneCode
    {
        get => _phoneCode.Base64Decode();
        set => _phoneCode = value;
    }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NikeName { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public UserGender? UserGender { get; set; }

    /// <summary>
    /// 用户邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadImage { get; set; }
}

/// <summary>
/// 注册时外部平台信息
/// </summary>
public class ExternalUserViewModel
{
    /// <summary>
    /// 其它平台名称
    /// </summary>
    public string Scheme { get; set; }

    /// <summary>
    /// 唯一标识符
    /// </summary>
    public string UniqueIdentifier { get; set; }

    /// <summary>
    /// 其它标识符
    /// </summary>
    public string OtherIdentifier { get; set; }

    /// <summary>
    /// 其它信息
    /// </summary>
    public dynamic OtherInformation { get; set; }
}