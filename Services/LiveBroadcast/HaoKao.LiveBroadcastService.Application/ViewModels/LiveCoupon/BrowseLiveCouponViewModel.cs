using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveCoupon;


[AutoMapFrom(typeof(Domain.Entities.LiveCoupon))]
public class BrowseLiveCouponViewModel : IDto
{

    public Guid Id { get; set; }
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
    /// 是否上架
    /// </summary>
    public bool IsShelves { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
