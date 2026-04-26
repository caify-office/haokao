namespace HaoKao.LiveBroadcastService.Domain.Entities;

public class LiveComment : AggregateRoot<Guid>,
                           IIncludeCreateTime,
                           IIncludeCreatorId<Guid>,
                           IIncludeCreatorName,
                           IIncludeMultiTenant<Guid>,
                           ITenantShardingTable
{
    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 评分
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 直播Id
    /// </summary>
    public Guid LiveId { get; set; }

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}