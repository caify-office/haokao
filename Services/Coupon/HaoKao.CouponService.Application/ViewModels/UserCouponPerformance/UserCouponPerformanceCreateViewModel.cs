using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;


[AutoMapTo(typeof(Domain.Commands.UserCouponPerformance.CreateUserCouponPerformanceCommand))]
public class CreateUserCouponPerformanceViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 订单编号
    /// </summary>
    [DisplayName("订单编号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string OrderNo{ get; set; }

    /// <summary>
    /// 订单id
    /// </summary>
    [DisplayName("订单id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid OrderId{ get; set; }

    /// <summary>
    /// 手机号码--冗余
    /// </summary>
    [DisplayName("手机号码--冗余")]

    public string TelPhone{ get; set; }

    /// <summary>
    /// 昵称--冗余
    /// </summary>
    [DisplayName("昵称--冗余")]
    public string NickName{ get; set; }

    /// <summary>
    /// 产品名称--冗余
    /// </summary>
    [DisplayName("产品名称--冗余")]
    [Required(ErrorMessage = "{0}不能为空")]

    public string ProductName{ get; set; }

    /// <summary>
    /// 实际支付金额--冗余
    /// </summary>
    [DisplayName("实际支付金额--冗余")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal FactAmount{ get; set; }

    /// <summary>
    /// 产品原价--冗余
    /// </summary>
    [DisplayName("产品原价--冗余")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal Amount{ get; set; }

    /// <summary>
    /// 支付时间--冗余
    /// </summary>
    [DisplayName("支付时间--冗余")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime PayTime{ get; set; }

    /// <summary>
    /// 备注--后台手动添加的默认手动添加
    /// </summary>
    [DisplayName("备注--后台手动添加的默认手动添加")]
    public string Remark{ get; set; }

    /// <summary>
    /// 助教实名名称
    /// </summary>
    [DisplayName("助教实名名称")]
    public string PersonName{ get; set; }

    /// <summary>
    /// 营销助教userid
    /// </summary>
    [DisplayName("营销助教userid")]
    public Guid PersonUserId{ get; set; }
}