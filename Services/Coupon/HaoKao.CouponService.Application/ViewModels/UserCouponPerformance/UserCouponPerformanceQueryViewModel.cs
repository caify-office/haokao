using HaoKao.CouponService.Domain.Queries;

namespace HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;


[AutoMapFrom(typeof(UserCouponPerformanceQuery))]
[AutoMapTo(typeof(UserCouponPerformanceQuery))]
public class UserCouponPerformanceQueryViewModel: QueryDtoBase<UserCouponPerformanceQueryListViewModel>
{

    /// <summary>
    /// 订单编号
    /// </summary>
    public string OrderNo { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }
    /// <summary>
    /// 销售人员名称
    /// </summary>
    public string PersonName { get; set; }
    /// <summary>
    /// 支付开始时间
    /// </summary>
    public string StartTime { get; set; }
    /// <summary>
    /// 支付结束时间
    /// </summary>
    public string EndTime { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    ///实际支付金额--冗余
    /// </summary>
    public decimal? FactAmount { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public string Resource { get; set; }

}

[AutoMapFrom(typeof(Domain.Models.UserCouponPerformance))]
[AutoMapTo(typeof(Domain.Models.UserCouponPerformance))]
public class UserCouponPerformanceQueryListViewModel : IDto
{

    public Guid Id { get; set; }
    /// <summary>
    /// 订单编号
    /// </summary>
    public string OrderNo{ get; set; }

    /// <summary>
    /// 订单id
    /// </summary>
    public Guid OrderId{ get; set; }

    /// <summary>
    /// 手机号码--冗余
    /// </summary>
    public string TelPhone{ get; set; }

    /// <summary>
    /// 昵称--冗余
    /// </summary>
    public string NickName{ get; set; }

    /// <summary>
    /// 产品名称--冗余
    /// </summary>
    public string ProductName{ get; set; }

    /// <summary>
    /// 实际支付金额--冗余
    /// </summary>
    public decimal FactAmount{ get; set; }

    /// <summary>
    /// 产品原价--冗余
    /// </summary>
    public decimal Amount{ get; set; }

    /// <summary>
    /// 支付时间--冗余
    /// </summary>
    public DateTime PayTime{ get; set; }

    /// <summary>
    /// 备注--后台手动添加的默认手动添加
    /// </summary>
    public string Remark{ get; set; }

    /// <summary>
    /// 助教实名名称
    /// </summary>
    public string PersonName{ get; set; }

    /// <summary>
    /// 营销助教userid
    /// </summary>
    public Guid PersonUserId{ get; set; }

    public DateTime CreateTime { get; set; }
}