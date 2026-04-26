namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 禁言用户
/// </summary>
public class MutedUser : AggregateRoot<Guid>,
                         IIncludeCreateTime,
                         IIncludeCreatorId<Guid>,
                         IIncludeCreatorName,
                         IIncludeMultiTenant<Guid>,
                         ITenantShardingTable
{
    /// <summary>
    /// 禁言用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 禁言用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 禁言用户手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}