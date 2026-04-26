using HaoKao.CouponService.Domain.Enumerations;

namespace HaoKao.CouponService.Application.ViewModels.Coupon;


[AutoMapFrom(typeof(Domain.Models.Coupon))]
public class BrowseCouponViewModel : IDto
{

    public Guid Id { get; set; }

    /// <summary>
    /// 有效期-开始时间
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 有效期-结束时间
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 优惠券卡号
    /// </summary>
    public string CouponCode { get; set; }

    /// <summary>
    /// 优惠券名称
    /// </summary>
    public string CouponName { get; set; }

    /// <summary>
    /// 优惠券说明
    /// </summary>
    public string CouponDesc { get; set; }

    /// <summary>
    /// 助教名称
    /// </summary>
    public string PersonName { get; set; }
    /// <summary>
    /// 助教id
    /// </summary>
    public Guid PersonUserId { get; set; }
    /// <summary>
    /// 优惠券类型 1-抵用券 2-折扣券  3-实名券
    /// </summary>
    public CouponTypeEnum CouponType { get; set; }

    /// <summary>
    /// 适用产品
    /// </summary>
    public List<Guid> ProductIds { get; set; }
    /// <summary>
    /// 产品包id
    /// </summary>
    public Guid ProductPackageId { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    public  string ProductName { get; set; }

    /// <summary>
    ///  金额/折扣
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 是否实名优惠券 true-实名优惠券 false--非实名优惠券即活动优惠券
    /// </summary>
    public bool IsOnlyName { get; set; }
    /// <summary>
    /// 小时
    /// </summary>
    public int Hour { get; set; }
    /// <summary>
    /// 日期选择类型
    /// </summary>
    public TimeTypeEnum TimeType { get; set; }
    /// <summary>
    /// 试用范围
    /// </summary>
    public ScopeEnum Scope { get; set; }

    /// <summary>
    ///门槛金额
    /// </summary>
    public decimal ThresholdAmount { get; set; }




}
