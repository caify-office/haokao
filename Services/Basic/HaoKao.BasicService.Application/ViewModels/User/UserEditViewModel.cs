namespace HaoKao.BasicService.Application.ViewModels.User;

public class UserEditViewModel : IDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// 用户登陆账号
    /// </summary>
    [DisplayName("用户登录账号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}最小长度为1")]
    [MaxLength(36, ErrorMessage = "{0}最大长度为36")]
    public string UserAccount { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    [DisplayName("用户名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}最小长度为1")]
    [MaxLength(50, ErrorMessage = "{0}最大长度为50")]
    public string UserName { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    [Required(ErrorMessage = "用户状态不能为空")]
    public DataState State { get; set; }

    /// <summary>
    /// 联系电话   
    /// 非必填
    /// </summary>
    public string ContactNumber { get; set; }

    /// <summary>
    /// 用户登陆密码
    /// </summary>
    [Required(ErrorMessage = "用户登录密码不能为空")]
    public string UserPassword { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; }
}