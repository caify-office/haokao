namespace HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;

[AutoMapFrom(typeof(Domain.Entities.RegisterUser))]
public class CreateRegisterUserViewModel : IDto
{
    /// <summary>
    /// 手机号码
    /// </summary>
    [DisplayName("手机号码")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [DisplayName("密码")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Password { get; set; }

    /// <summary>
    /// 公众Id
    /// </summary>
    [DisplayName("公众Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string OpenId { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    [DisplayName("用户性别")]
    [Required(ErrorMessage = "{0}不能为空")]
    public UserGender UserGender { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    [DisplayName("用户昵称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string NickName { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    [DisplayName("用户状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public UserState UserState { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    [DisplayName("最后登录IP")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    [DisplayName("最后登录时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 注册IP
    /// </summary>
    [DisplayName("注册IP")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string RegisterIp { get; set; }

    /// <summary>
    /// 注册时间
    /// </summary>
    [DisplayName("注册时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime RegisterTime { get; set; }
}