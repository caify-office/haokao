namespace HaoKao.BasicService.Application.ViewModels.User;

/// <summary>
/// 考试中切换考试时，用户名密码直接登录使用
/// </summary>
public class ChangeExamViewModel
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