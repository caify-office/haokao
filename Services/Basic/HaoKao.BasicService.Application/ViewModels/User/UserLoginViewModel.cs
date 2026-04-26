namespace HaoKao.BasicService.Application.ViewModels.User;

/// <summary>
/// 登陆第一步验证
/// </summary>
public class UserLoginStepOneViewModel
{
    private string _userAccount;

    /// <summary>
    /// 登陆名称
    /// </summary>
    [DisplayName("登陆名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string UserAccount
    {
        get => _userAccount.Base64Decode();
        set => _userAccount = value;
    }

    /// <summary>
    /// 登陆的考试
    /// </summary>
    [DisplayName("登陆的考试")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid TenantId { get; set; }

    private string _password;

    /// <summary>
    /// 登陆密码
    /// </summary>
    [DisplayName("登陆密码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Password
    {
        get => _password.Base64Decode();
        set => _password = value;
    }

    [Required(ErrorMessage = "系统功能模块代码不能为空")]
    public SystemModule SystemModule { get; set; }

    private string _examBehavior;

    [DisplayName("考试行为")]
    [Required(ErrorMessage = "{0} 不能为空")]
    public string ExamBehavior
    {
        get => _examBehavior.Base64Decode();
        set => _examBehavior = value;
    }
}

/// <summary>
/// 第一步验证完后返回结果
/// </summary>
public class UserLoginStepOneReturnViewModel
{
    public string Phone { get; set; }

    public string ValidString { get; set; }
}

/// <summary>
/// 用户登陆第二步验证
/// </summary>
public class UserLoginSecondStepViewModel : UserLoginStepOneViewModel
{
    private string _phone;

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

    private string _phoneCode;

    /// <summary>
    /// 手机验证码
    /// </summary>
    [DisplayName("手机验证码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string PhoneCode
    {
        get => _phoneCode.Base64Decode();
        set => _phoneCode = value;
    }
}