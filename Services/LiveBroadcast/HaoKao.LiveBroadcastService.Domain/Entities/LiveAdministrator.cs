namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 直播管理员
/// </summary>
public class LiveAdministrator : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}