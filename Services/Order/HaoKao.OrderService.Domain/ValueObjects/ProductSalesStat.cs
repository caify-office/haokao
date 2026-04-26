namespace HaoKao.OrderService.Domain.ValueObjects;

/// <summary>
/// 产品销售统计
/// </summary>
/// <param name="ProductId">产品Id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="SalesCount">销售量</param>
/// <param name="SalesRevenue">销售金额(元)</param>
/// <param name="TenantId"></param>
public record ProductSalesStat(Guid ProductId, string ProductName, int SalesCount, decimal SalesRevenue, Guid TenantId);

/// <summary>
/// 产品销售统计列表
/// </summary>
/// <param name="RecordCount">记录数</param>
/// <param name="Result">数据</param>
public record ProductSalesStatList(int RecordCount, IReadOnlyList<ProductSalesStat> Result);