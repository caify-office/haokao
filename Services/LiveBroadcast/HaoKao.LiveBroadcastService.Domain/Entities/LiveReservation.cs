using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 直播预约
/// </summary>
public class LiveReservation : AggregateRoot<Guid>,
                               IIncludeCreateTime,
                               IIncludeCreatorId<Guid>,
                               IIncludeCreatorName,
                               IIncludeMultiTenant<Guid>,
                               ITenantShardingTable

{
    /// <summary>
    /// 预约直播产品id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 预约视频直播Id
    /// </summary>
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 预约来源
    /// </summary>
    public ReservationSource ReservationSource { get; set; }

    /// <summary>
    /// 是否已通知
    /// </summary>
    public bool Notified { get; set; }

    /// <summary>
    /// 微信用户的OpenId
    /// </summary>
    public string OpenId { get; set; }

    /// <summary>
    /// 预约时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    ///用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}