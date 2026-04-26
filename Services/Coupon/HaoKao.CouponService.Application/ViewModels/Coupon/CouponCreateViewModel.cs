using HaoKao.CouponService.Domain.Enumerations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.CouponService.Application.ViewModels.Coupon;


[AutoMapTo(typeof(Domain.Commands.Coupon.CreateCouponCommand))]
public class CreateCouponViewModel : IDto
{
    /// <summary>
    /// 有效期-开始时间
    /// </summary>
    [DisplayName("有效期-开始时间")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 有效期-结束时间
    /// </summary>
    [DisplayName("有效期-结束时间")]
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 优惠券卡号
    /// </summary>
    [DisplayName("优惠券卡号")]

    public string CouponCode { get; set; }

    /// <summary>
    /// 优惠券名称
    /// </summary>
    [DisplayName("优惠券名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string CouponName { get; set; }

    /// <summary>
    /// 优惠券说明
    /// </summary>
    [DisplayName("优惠券说明")]
    public string CouponDesc { get; set; }

    /// <summary>
    /// 助教名称
    /// </summary>
    [DisplayName("助教名称")]
    public string PersonName { get; set; }

    /// <summary>
    /// 助教userid
    /// </summary>
    [DisplayName("助教userid")]
    public Guid PersonUserId { get; set; }

    /// <summary>
    /// 优惠券类型 1-抵用券 2-折扣券
    /// </summary>
    [DisplayName("优惠券类型 1-抵用券 2-折扣券")]
    [Required(ErrorMessage = "{0}不能为空")]
    public CouponTypeEnum CouponType { get; set; }

    /// <summary>
    /// 适用产品包
    /// </summary>
    [DisplayName("适用产品包")]
    public List<Guid> ProductIds { get; set; }

    [DisplayName("产品包id")]
    public Guid ProductPackageId { get; set; }

    [DisplayName("冗余产品名称")]
    public  string ProductName { get; set; }

    /// <summary>
    /// 金额/折扣
    /// </summary>
    [DisplayName("金额/折扣")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal Amount { get; set; }

    /// <summary>
    /// 是否实名优惠券 true-实名优惠券 false--非实名优惠券即活动优惠券
    /// </summary>
    [DisplayName("是否实名优惠券 true-实名优惠券 false--非实名优惠券即活动优惠券")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsOnlyName { get; set; }


    /// <summary>
    /// 1-按日期 2-按小时
    /// </summary>
    [DisplayName("有效期选择类型")]
    public TimeTypeEnum TimeType { get; set; }
    /// <summary>
    /// 小时
    /// </summary>
    [DisplayName("小时")]
    public int Hour { get; set; }

    /// <summary>
    /// 适用范围
    /// </summary>
    [DisplayName("适用范围")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ScopeEnum Scope { get; set; }
    /// <summary>
    /// 门槛金额
    /// </summary>
    [DisplayName("门槛金额")]

    public decimal ThresholdAmount { get; set; }

}