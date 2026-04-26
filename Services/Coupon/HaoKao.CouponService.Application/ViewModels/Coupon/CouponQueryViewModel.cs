using HaoKao.CouponService.Domain.Enumerations;
using HaoKao.CouponService.Domain.Queries;

namespace HaoKao.CouponService.Application.ViewModels.Coupon;


[AutoMapFrom(typeof(CouponQuery))]
[AutoMapTo(typeof(CouponQuery))]
public class CouponQueryViewModel: QueryDtoBase<CouponQueryListViewModel>
{
    /// <summary>
    /// 优惠券code
    /// </summary>
    public string CouponCode { get; set; }
    /// <summary>
    /// 优惠券名称
    /// </summary>
    public string CouponName { get; set; }
    /// <summary>
    /// 下单用户名称
    /// </summary>
    public string OrderUserName { get; set; }
    /// <summary>
    /// 类型
    /// </summary>

    public CouponTypeEnum? CouponType { get; set; }

    /// <summary>
    /// 过滤id数组
    /// </summary>
    public string ExcludeIds { get; set; }
    /// <summary>
    /// 是否过滤实名优惠卷（传true：过滤掉实名优惠卷，不传或者传false：不过滤实名优惠卷）
    /// </summary>
    public bool? IsFilterSmCoupon { get; set; }
    /// <summary>
    /// 是否过期 (true:返回已过期优惠卷，false：返回未过期优惠卷，不传：过期和未过期的都返回)
    /// </summary>
    public bool? IsExpired { get; set; }

}

[AutoMapFrom(typeof(Domain.Models.Coupon))]
[AutoMapTo(typeof(Domain.Models.Coupon))]
public class CouponQueryListViewModel : IDto
{

    public Guid Id { get; set; }
    /// <summary>
    /// 有效期-开始时间
    /// </summary>
    public DateTime EndDate{ get; set; }

    /// <summary>
    /// 有效期-结束时间
    /// </summary>
    public DateTime BeginDate{ get; set; }

    /// <summary>
    /// 优惠券卡号
    /// </summary>
    public string CouponCode{ get; set; }

    /// <summary>
    /// 优惠券名称
    /// </summary>
    public string CouponName{ get; set; }

    /// <summary>
    /// 优惠券说明
    /// </summary>
    public string CouponDesc{ get; set; }

    /// <summary>
    /// 助教名称
    /// </summary>
    public string PersonName{ get; set; }

    /// <summary>
    /// 优惠券类型 1-抵用券 2-折扣券
    /// </summary>
    public CouponTypeEnum CouponType{ get; set; }

    /// <summary>
    /// 适用产品
    /// </summary>
    public List<Guid> ProductIds{ get; set; }

    /// <summary>
    ///  金额/折扣
    /// </summary>
    public decimal Amount{ get; set; }

    /// <summary>
    /// 是否实名优惠券 true-实名优惠券 false--非实名优惠券即活动优惠券
    /// </summary>
    public bool IsOnlyName{ get; set; }

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
    /// <summary>
    /// 对应的优惠券使用记录
    /// </summary>
    public List<Domain.Models.UserCoupon> UserCoupons{ get; set; }
    /// <summary>
    /// 使用数
    /// </summary>
    public int UseCount { get; set; }
    /// <summary>
    /// 领取数
    /// </summary>
    public  int  ReceiveCount { get; set; }
}