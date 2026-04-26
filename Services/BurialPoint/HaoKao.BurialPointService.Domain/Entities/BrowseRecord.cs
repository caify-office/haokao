namespace HaoKao.BurialPointService.Domain.Entities;

/// <summary>
/// 浏览记录
/// </summary>
public class BrowseRecord : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>,IIncludeCreatorId<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 埋点id
    /// </summary>
    public Guid BurialPointId { get; set; }
    /// <summary>
    /// 用户昵称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 浏览信息
    /// </summary>
    public string BrowseData { get; set; }

    /// <summary>
    /// 是否付费用户
    /// </summary>
    public bool IsPaidUser { get; set; }

    /// <summary>
    /// 浏览时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }
}
