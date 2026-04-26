
using HaoKao.CouponService.Application.ViewModels.Coupon;
using System.Text.Json.Serialization;

namespace HaoKao.CouponService.Application.ViewModels.UserCoupon;
[AutoMapFrom(typeof(Domain.Models.UserCoupon))]
public class BrowseUserCouponViewModel : IDto
{
    /// <summary>
    /// 优惠券实体
    /// </summary>
    public BrowseCouponViewModel coupon { get; set; }

    /// <summary>
    /// 优惠券id
    /// </summary>
    public Guid CouponId{ get; set; }
    /// <summary>
    /// 订单id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// 订单编号
    /// </summary>
    public string OrderNo { get; set; }

    /// <summary>
    /// 是否使用 0-未使用 1-已使用
    /// </summary>
    public bool IsUse{ get; set; }

    /// <summary>
    /// 手机号码--冗余
    /// </summary>
    public string TelPhone { get; set; }

    /// <summary>
    ///昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    ///产品名称
    /// </summary>
    public string ProductName { get; set; }

    public Guid ProductId { get; set; }
    /// <summary>
    ///时机支付金额
    /// </summary>
    public decimal FactAmount { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    [JsonIgnore]
    public DateTime EndTime { get; set; }


    public DateTime EndDate 
    { 
        get
        {
            return EndTime;
        } 
    }

    public DateTime CreateTime { get; set; }
}
