using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 直播消息类
/// </summary>
public partial class LiveMessage : AggregateRoot<Guid>,
                                   IIncludeCreatorId<Guid>,
                                   IIncludeCreatorName,
                                   IIncludeCreateTime,
                                   IIncludeMultiTenant<Guid>,
                                   ITenantShardingTable
{
    /// <summary>
    /// 直播间Id
    /// </summary>
    public Guid LiveId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public LiveMessageType LiveMessageType { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}