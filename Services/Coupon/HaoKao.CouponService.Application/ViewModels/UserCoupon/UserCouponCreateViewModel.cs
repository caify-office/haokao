using HaoKao.CouponService.Domain.Enumerations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HaoKao.CouponService.Application.ViewModels.UserCoupon;


[AutoMapTo(typeof(Domain.Commands.UserCoupon.CreateUserCouponCommand))]
public class CreateUserCouponViewModel : IDto
{

    /// <summary>
    /// 优惠券id
    /// </summary>
    [DisplayName("优惠券id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string CouponId{ get; set; }

    /// <summary>
    /// 是否使用 0-未使用 1-已使用
    /// </summary>
    [DisplayName("是否使用 0-未使用 1-已使用")]
    public bool IsUse{ get; set; }

    /// <summary>
    /// 订单id
    /// </summary>
    [DisplayName("订单id")]
    public Guid OrderId { get; set; }

    /// <summary>
    /// 订单编号
    /// </summary>
    [DisplayName("订单编号")]
    public string OrderNo { get; set; }


    /// <summary>
    /// 手机号码--冗余
    /// </summary>
    [DisplayName("手机号码")]
    public string TelPhone { get; set; }

    /// <summary>
    ///昵称
    /// </summary>
    [DisplayName("昵称")]
    public string NickName { get; set; }

    /// <summary>
    ///产品名称
    /// </summary>
    [DisplayName("产品名称")]
    public string ProductName { get; set; }

    [DisplayName("产品ID")]
    public Guid ProductId { get; set; }

    /// <summary>
    ///实际支付金额
    /// </summary>
    [DisplayName("实际支付金额")]
    public decimal FactAmount { get; set; }


    [DisplayName("支付金额")]
    public decimal Amount { get; set; }

    [DisplayName("支付时间")]
    public DateTime PayTime { get; set; }


    /// <summary>
    ///备注
    /// </summary>
    [DisplayName("备注")]
    public string  Remark { get; set; }

    /// <summary>
    /// 渠道类型
    /// </summary>
    [JsonIgnore]
    [DisplayName("备注")]
    public ChannelType ChannelType { get; set; }


    /// <summary>
    /// 微信用户的OpenId
    /// </summary>
    [DisplayName("备注")]
    public string OpenId { get; set; }


}