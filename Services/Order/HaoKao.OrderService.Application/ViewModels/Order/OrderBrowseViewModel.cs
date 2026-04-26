using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.Order;

[AutoMapFrom(typeof(Domain.Entities.Order))]
public class BrowseOrderViewModel : IDto
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
    /// 订单流水号
    /// </summary>
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 购买的产品Id
    /// </summary>
    public Guid PurchaseProductId { get; set; }

    /// <summary>
    /// 购买产品名称
    /// </summary>
    public string PurchaseName { get; set; }

    /// <summary>
    /// 产品类型
    /// </summary>
    public PurchaseProductType PurchaseProductType { get; set; }

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
    /// 购买的产品详细内容
    /// </summary>
    public List<PurchaseProductContent> PurchaseProductContents { get; set; } = [];
}