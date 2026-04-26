namespace HaoKao.OrderService.Domain.Entities;

public class ProductSales : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal ActualPrice { get; init; }

    /// <summary>
    /// 交易时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}