using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.ValueObjects;

namespace HaoKao.OrderService.Domain.Statistics;

public interface ISalesStatisticsFactory : IManager
{
    ISalesStatistics Create(SalesStatDimension dimension);
}

public interface ISalesStatistics
{
    SalesStatDimension Dimension { get; }

    Task<SalesStatisticsList> GetSalesStatList(DateOnly? startDate, DateOnly? endDate, int pageIndex = 1, int pageSize = 10, params Guid[] tenantIds);

    Task<SalesStatisticsListDetail> GetSalesStatListDetail(DateOnly startDate, DateOnly endDate, int pageIndex = 1, int pageSize = 10, params Guid[] tenantIds);
}