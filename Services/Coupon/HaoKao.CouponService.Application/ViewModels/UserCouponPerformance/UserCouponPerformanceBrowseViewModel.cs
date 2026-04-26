namespace HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;


[AutoMapFrom(typeof(Domain.Models.UserCouponPerformance))]
public class BrowseUserCouponPerformanceViewModel : IDto
{

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
}
