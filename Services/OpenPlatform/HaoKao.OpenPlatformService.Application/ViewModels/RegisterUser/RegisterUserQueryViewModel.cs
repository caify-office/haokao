using HaoKao.OpenPlatformService.Domain.Queries;

namespace HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;

[AutoMapFrom(typeof(RegisterUserQuery))]
[AutoMapTo(typeof(RegisterUserQuery))]
public class RegisterUserQueryViewModel : QueryDtoBase<RegisterUserQueryListViewModel>
{
    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserState? UserState { get; set; }

    /// <summary>
    /// 开始注册时间
    /// </summary>
    public DateTime? StartRegisterDateTime { get; set; }

    /// <summary>
    /// 结束注册时间
    /// </summary>
    public DateTime? EndRegisterDateTime { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.RegisterUser))]
[AutoMapTo(typeof(Domain.Entities.RegisterUser))]
public class RegisterUserQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public UserGender UserGender { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserState UserState { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string EmailAddress { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadImage { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 注册客户端Id
    /// </summary>
    public string ClientId { get; set; }
}