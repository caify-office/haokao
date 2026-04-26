using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveCoupon;


[AutoMapTo(typeof(Domain.Commands.LiveCoupon.UpdateLiveCouponModel))]
public class UpdateLiveCouponViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    [DisplayName("所属视频直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 优惠卷Id
    /// </summary>
    [DisplayName("优惠卷Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveCouponId { get; set; }

    /// <summary>
    /// 优惠卷名称
    /// </summary>
    [DisplayName("优惠卷名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string LiveCouponName { get; set; }

    /// <summary>
    /// 金额/折扣--合并一个字段  折扣85折显示0.85
    /// </summary>
    [DisplayName("金额/折扣--合并一个字段  折扣85折显示0.85")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal Amount { get; set; }

    /// <summary>
    /// 优惠券类型 1-抵用券 2-折扣券
    /// </summary>
    [DisplayName("优惠券类型 1-抵用券 2-折扣券")]
    [Required(ErrorMessage = "{0}不能为空")]
    public CouponTypeEnum CouponType { get; set; }

    /// <summary>
    /// 适用范围
    /// </summary>
    [DisplayName("适用范围")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ScopeEnum Scope { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    [DisplayName("是否上架")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsShelves { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; set; }
}