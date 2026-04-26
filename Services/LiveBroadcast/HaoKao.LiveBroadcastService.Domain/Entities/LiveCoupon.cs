using HaoKao.LiveBroadcastService.Domain.Enums;
using System.Text.Json.Serialization;

namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 直播优惠卷
/// </summary>
public class LiveCoupon : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 所属视频直播
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public LiveVideo LiveVideo { get; set; }

    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 优惠卷Id
    /// </summary>
    public Guid LiveCouponId { get; set; }

    /// <summary>
    /// 优惠卷名称
    /// </summary>
    public string LiveCouponName { get; set; }

    /// <summary>
    /// 金额/折扣--合并一个字段  折扣85折显示0.85
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 优惠券类型 1-抵用券 2-折扣券
    /// </summary>
    public CouponTypeEnum CouponType { get; set; }

    /// <summary>
    /// 适用范围
    /// </summary>
    public ScopeEnum Scope { get; set; }

    /// <summary>
    /// 创建时间（上架时间）
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 是否上架
    /// </summary>
    public bool IsShelves { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}