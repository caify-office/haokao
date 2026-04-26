using HaoKao.BasicService.Domain.Enumerations;

namespace HaoKao.BasicService.Domain.Entities;

public class User : AggregateRoot<Guid>,
                    IIncludeInitField,
                    IIncludeMultiTenant<Guid>,
                    IIncludeCreateTime,
                    IIncludeCreatorId<Guid>,
                    IIncludeMultiTenantName,
                    IIncludeCreatorName
{
    /// <summary>
    /// 用户账户
    /// </summary>
    public string UserAccount { get; set; }

    /// <summary>
    /// 用户密码
    /// </summary>
    public string UserPassword { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 用户联系方式
    /// </summary>
    public string ContactNumber { get; set; }

    /// <summary>
    /// 绑定其它相关服务的关键标识Id
    /// </summary>
    public Guid OtherId { get; set; }

    /// <summary>
    /// 用户的状态
    /// </summary>
    public DataState State { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 授权类型
    /// </summary>
    public AuthorizeType AuthorizeType { get; set; }

    /// <summary>
    /// 是否初始化数据
    /// </summary>
    public bool IsInitData { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    public DateTime CreateTime { get; set; }

    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }

    public virtual List<Role> Roles { get; set; } = [];

    /// <summary>
    /// 设置的用户规则
    /// </summary>
    public List<UserRule> RulesList { get; set; } = [];
}