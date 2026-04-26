namespace HaoKao.BasicService.Application.ViewModels.User;

[AutoMapFrom(typeof(Domain.Entities.User))]
[AutoMapTo(typeof(Domain.Entities.User))]
public class UserLoginDetailViewModel : IDto
{
    /// <summary>
    /// 用户登陆名称
    /// </summary>

    public string UserAccount { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 用户角色
    /// </summary>
    public List<RoleDetailViewModel> UserRoles { get; set; }

    /// <summary>
    /// 用户手机号
    /// </summary>
    public string ContactNumber { get; set; }

    /// <summary>
    /// token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpiresHours
    {
        get
        {
            var config = EngineContext.Current.GetAppModuleConfig<AuthorizeConfig>();
            return DateTime.Now.AddHours(config.JwtConfig.ExpiresHours);
        }
    }

    /// <summary>
    /// 强制执行修改密码
    /// </summary>
    public bool EnforceChangePassword { get; set; }
}