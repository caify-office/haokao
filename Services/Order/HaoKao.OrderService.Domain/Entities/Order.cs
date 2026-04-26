using HaoKao.OrderService.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaoKao.OrderService.Domain.Entities;

/// <summary>
/// 订单表
/// </summary>
public class Order : AggregateRoot<Guid>,
                     IIncludeCreatorId<Guid>,
                     IIncludeCreateTime,
                     IIncludeUpdateTime,
                     IIncludeMultiTenant<Guid>,
                     ITenantShardingTable,
                     IIncludeCreatorName,
                     IIncludeMultiTenantName
{
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
    /// 订单号,主要是和第三方平交易时产生的商户交易号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 购买的产品Id(购买产品包下面的产品,这里传产品包id，购买单个产品，这个传产品id)
    /// </summary>
    public Guid PurchaseProductId { get; set; }

    /// <summary>
    /// 购买产品名称(购买产品包下面的产品,这里传产品包名称，购买单个产品，这个传产品名称)
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
    /// 苹果内购是否恢复购买过来的
    /// </summary>
    public bool IosRestorePurchase { get; set; }

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
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 客户端Id ，后续备用
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 客户端名称，后续备用
    /// </summary>
    public string ClientName { get; set; }

    /// <summary>
    /// 购买的产品详细内容
    /// </summary>
    [NotMapped]
    public List<PurchaseProductContent> PurchaseProductContents { get; set; } = [];

    /// <summary>
    /// 订单发票
    /// </summary>
    public OrderInvoice OrderInvoice { get; set; }
}

/// <summary>
/// 购买的产品内容
/// </summary>
public class PurchaseProductContent
{
    /// <summary>
    /// 内容Id
    /// </summary>
    public Guid ContentId { get; set; }

    /// <summary>
    /// 内容名称
    /// </summary>
    public string ContentName { get; set; }

    /// <summary>
    /// 产品类型
    /// </summary>
    public ContentType ContentType { get; set; }

    /// <summary>
    /// 原价
    /// </summary>
    public decimal ContentAmount { get; set; }

    /// <summary>
    /// 优惠价
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 优惠券减免价格
    /// </summary>
    public decimal CouponAmount { get; set; }

    /// <summary>
    /// 成交价格
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 苹果内购产品Id
    /// </summary>
    public string AppleInPurchaseProductId { get; set; }

    /// <summary>
    /// 0-按日期 1-按天数
    /// </summary>
    public int ExpiryTimeTypeEnum { get; set; }

    /// <summary>
    /// 按天数
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 优惠券集合
    /// </summary>
    public List<CouponViewModel> Coupons { get; set; }
}

public class CouponViewModel
{
    /// <summary>
    /// 优惠券id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 优惠券名称
    /// </summary>
    public string CouponName { get; set; }

    /// <summary>
    /// 优惠券类型
    /// </summary>
    public int CouponType { get; set; }

    /// <summary>
    /// 折扣/金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 金额加密
    /// </summary>
    public string AmountEncryption { get; set; }
}