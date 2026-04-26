namespace HaoKao.OrderService.Domain.ValueObjects;

public record SalesStatisticsList
{
    public int RecordCount { get; init; }

    public IReadOnlyList<SalesStatisticsItem> Items { get; init; }
}

public record SalesStatisticsItem
{
    /// <summary>
    /// 日期
    /// </summary>
    public string Date { get; init; }

    /// <summary>
    /// 订单数
    /// </summary>
    public int SumOrderCount { get; init; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal SumOrderAmount { get; init; }
}