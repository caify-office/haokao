namespace HaoKao.OrderService.Domain.ValueObjects;

public record SalesStatisticsListDetail
{
    public int RecordCount { get; init; }

    public IReadOnlyList<SalesStatisticsItemDetail> Items { get; init; }
}

public record SalesStatisticsItemDetail
{
    /// <summary>
    /// 使用的平台配置的支付者的名称
    /// </summary>
    public string PlatformPayerName { get; init; }

    /// <summary>
    /// 订单流水号
    /// </summary>
    public string OrderSerialNumber { get; init; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNumber { get; init; }

    /// <summary>
    /// 购买产品名称
    /// </summary>
    public string PurchaseName { get; init; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal OrderAmount { get; init; }

    /// <summary>
    /// 实际金额
    /// </summary>
    public decimal ActualAmount { get; init; }

    /// <summary>
    /// 下单用户ID
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 下单用户名称
    /// </summary>
    public string CreatorName { get; init; }

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 订单更新时间，完成支付时间
    /// </summary>
    public DateTime UpdateTime { get; init; }
}