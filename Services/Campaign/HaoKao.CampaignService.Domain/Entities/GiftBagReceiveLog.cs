using HaoKao.CampaignService.Domain.Enums;

namespace HaoKao.CampaignService.Domain.Entities;

/// <summary>
/// 礼品包领取记录
/// </summary>
public sealed class GiftBagReceiveLog : AggregateRoot<Guid>,
                                        IIncludeMultiTenant<Guid>,
                                        ITenantShardingTable
{
    /// <summary>
    /// 礼品包Id
    /// </summary>
    public Guid GiftBagId { get; init; }

    /// <summary>
    /// 活动名称
    /// </summary>
    public string CampaignName { get; init; }

    /// <summary>
    /// 礼包类型
    /// </summary>
    public GiftType GiftType { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 领取时间
    /// </summary>
    public DateTime ReceiveTime { get; init; }

    /// <summary>
    /// 领取人Id
    /// </summary>
    public Guid ReceiverId { get; init; }

    /// <summary>
    /// 领取人名称
    /// </summary>
    public string ReceiverName { get; init; }

    /// <summary>
    /// 注册时间
    /// </summary>
    public DateTime RegistrationTime { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}