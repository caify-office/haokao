using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.CouponService.Application.ViewModels.UserCoupon;


[AutoMapTo(typeof(Domain.Commands.UserCoupon.UpdateUserCouponCommand))]
public class UpdateUserCouponViewModel : IDto
{
    public Guid  Id { get; set; }
    /// <summary>
    /// 是否使用 0-未使用 1-已使用
    /// </summary>
    [DisplayName("是否使用 0-未使用 1-已使用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsUse { get; set; }

    /// <summary>
    /// 订单id
    /// </summary>
    [DisplayName("订单Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid OrderId { get; set; }
    /// <summary>
    /// 订单编号
    /// </summary>
    [DisplayName("订单编号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string  OrderNo { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string ProductName { get; set; }

    /// <summary>
    ///原价
    /// </summary>
    [DisplayName("原价")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal Amount { get; set; }
    /// <summary>
    ///支付时间
    /// </summary>
    [DisplayName("支付时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime PayTime { get; set; }
}