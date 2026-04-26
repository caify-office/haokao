using Girvs.BusinessBasis.Queries;
using HaoKao.CouponService.Application.ViewModels.Coupon;
using HaoKao.CouponService.Domain.Queries;
using System.Text.Json.Serialization;

namespace HaoKao.CouponService.Application.ViewModels.UserCoupon;


[AutoMapFrom(typeof(UserCouponQuery))]
[AutoMapTo(typeof(UserCouponQuery))]
public class UserCouponQueryViewModel: QueryDtoBase<UserCouponQueryListViewModel>
{
    /// <summary>
    /// 昵称
    /// </summary>
    public  string    NickName { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public string StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public string EndTime { get; set; }
    /// <summary>
    /// 用户id
    /// </summary>
    public  Guid? UserId { get; set; }

    /// <summary>
    /// 是否使用
    /// </summary>
    public bool? IsUse { get; set; }

    /// <summary>
    /// 是否锁定
    /// </summary>
    public bool? IsLock { get; set; }

    /// <summary>
    /// 是否需要过滤过期优惠卷
    /// </summary>
    [QueryCacheKey]
    public bool? IsFilterExpired { get; set; }

    /// <summary>
    /// 优惠券id
    /// </summary>
    public Guid? CouponId { get; set; }
 
}

[AutoMapFrom(typeof(Domain.Models.UserCoupon))]
[AutoMapTo(typeof(Domain.Models.UserCoupon))]
public class UserCouponQueryListViewModel : IDto
{

    public Guid Id { get; set; }
    /// <summary>
    /// 优惠券id
    /// </summary>
    public Guid CouponId { get; set; }

    /// <summary>
    /// 优惠券实体
    /// </summary>
    public BrowseCouponViewModel coupon { get; set; }

    /// <summary>
    /// 是否使用 0-未使用 1-已使用
    /// </summary>
    public bool IsUse { get; set; }

    /// <summary>
    /// 订单id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// 订单编号
    /// </summary>
    public string OrderNo { get; set; }

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
    /// <summary>
    ///实际支付金额
    /// </summary>
    public decimal FactAmount { get; set; }

    /// <summary>
    ///原价
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    ///支付时间
    /// </summary>
    public DateTime PayTime { get; set; }

    /// <summary>
    ///来源
    /// </summary>
    public string Remark { get; set; }
    /// <summary>
    ///领取时间
    /// </summary>
    public DateTime CreateTime { get; set; }


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


}