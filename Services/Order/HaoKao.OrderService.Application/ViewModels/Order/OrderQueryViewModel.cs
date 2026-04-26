using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Queries;

namespace HaoKao.OrderService.Application.ViewModels.Order;

[AutoMapFrom(typeof(OrderQuery))]
[AutoMapTo(typeof(OrderQuery))]
public class OrderQueryViewModel : QueryDtoBase<OrderQueryListViewModel>
{
    /// <summary>
    /// 使用的平台配置的支付者的Id
    /// </summary>
    public Guid? PlatformPayerId { get; set; }

    /// <summary>
    /// 订单号(模糊搜索)
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 订单号(精确搜索)
    /// </summary>
    public string OrderNumber2 { get; set; }

    /// <summary>
    /// 订单流水号(模糊搜索)
    /// </summary>
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 订单流水号(精确搜索)
    /// </summary>
    public string OrderSerialNumber2 { get; set; }

    /// <summary>
    /// 购买产品名称
    /// </summary>
    public string PurchaseName { get; set; }

    /// <summary>
    /// 下单用户名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderState? OrderState { get; set; }

    /// <summary>
    /// 下单开始时间
    /// </summary>
    public DateTime? StartCreateTime { get; set; }

    /// <summary>
    /// 下单结束时间
    /// </summary>
    public DateTime? EndCreateTime { get; set; }

    /// <summary>
    /// 支付开始时间
    /// </summary>
    public DateTime? StartUpdateTime { get; set; }

    /// <summary>
    /// 支付结束时间
    /// </summary>
    public DateTime? EndUpdateTime { get; set; }

    /// <summary>
    /// 苹果内购恢复购买的
    /// </summary>
    public bool? IosRestorePurchase { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid? CreatorId { get; set; }

    /// <summary>
    /// 直播id（可为空）
    /// </summary>
    public Guid? LiveId { get; set; }

    /// <summary>
    /// 是否付费订单（true:返回实缴金额大于0订单，false：返回实缴金额等于0订单，不传都返回）
    /// </summary>
    public bool? IsPaidOrder { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.Order))]
[AutoMapTo(typeof(Domain.Entities.Order))]
public class OrderQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 使用的平台配置的支付者的Id
    /// </summary>
    public Guid PlatformPayerId { get; set; }

    /// <summary>
    /// 使用的平台配置的支付者的名称
    /// </summary>
    public string PlatformPayerName { get; set; }

    /// <summary>
    /// 购买的产品Id
    /// </summary>
    public Guid PurchaseProductId { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 订单流水号
    /// </summary>
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 购买产品名称
    /// </summary>
    public string PurchaseName { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// 实际金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 直播id（可为空）
    /// </summary>
    public Guid? LiveId { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderState OrderState { get; set; }

    /// <summary>
    /// 下单用户ID
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 下单用户名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 订单更新时间，完成支付时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 苹果内购恢复购买的
    /// </summary>
    public bool IosRestorePurchase { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public PurchaseProductType PurchaseProductType { get; set; }

    /// <summary>
    /// 订单过期失效时间
    /// </summary>
    public DateTime VaildTime { get; set; }

    /// <summary>
    /// 购买的产品详细内容
    /// </summary>
    public List<PurchaseProductContent> PurchaseProductContents { get; set; } = [];

    /// <summary>
    /// 实名优惠券集合
    /// </summary>
    public List<Guid> Coupons => PurchaseProductContents?.SelectMany(x => x.Coupons ?? []).Where(x => x.CouponType == 3).Select(x => x.Id).ToList() ?? [];

    /// <summary>
    /// 订单发票
    /// </summary>
    public MyOrderInvoiceViewModel OrderInvoice { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.OrderInvoice))]
public class MyOrderInvoiceViewModel : IDto
{
    /// <summary>
    /// 申请状态
    /// </summary>
    public RequestState RequestState { get; set; }

    /// <summary>
    /// 发票形式
    /// </summary>
    public InvoiceFormat InvoiceFormat { get; set; }

    /// <summary>
    /// 物流单号
    /// </summary>
    public string ShippingNumber { get; set; }

    /// <summary>
    /// 物流公司
    /// </summary>
    public string LogisticsCompany { get; set; }
}