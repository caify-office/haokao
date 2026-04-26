using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Statistics;
using HaoKao.OrderService.Domain.ValueObjects;

namespace HaoKao.OrderService.Infrastructure.Statistics;

public class SalesStatisticsFactory(IEnumerable<ISalesStatistics> instances) : ISalesStatisticsFactory
{
    public ISalesStatistics Create(SalesStatDimension dimension)
    {
        return instances.First(x => x.Dimension == dimension);
    }
}

public abstract class AbstractSalesStatistics(OrderDbContext dbContext) : ISalesStatistics
{
    public abstract SalesStatDimension Dimension { get; }

    protected abstract string DateFormat { get; }

    protected abstract string StartTimeFilter(DateOnly? startTime);

    protected abstract string EndTimeFilter(DateOnly? endTime);

    private string BuildWhere(DateOnly? startTime, DateOnly? endTime)
    {
        var builder = new StringBuilder($"WHERE t.OrderState = {(int)OrderState.PaymentSuccessful}");
        builder.Append(StartTimeFilter(startTime));
        builder.Append(EndTimeFilter(endTime));
        return builder.ToString().TrimEnd();
    }

    private string SalesStatisticsSql(DateOnly? startTime, DateOnly? endTime, List<string> tableNames)
    {
        var sqlWhere = BuildWhere(startTime, endTime);

        var unionSql = string.Join("\nUNION ALL\n", tableNames.Select(t => $"""
                                                                            SELECT t.`Date`,
                                                                                   COUNT(0) OrderCount,
                                                                                   SUM(t.ActualAmount) OrderAmount
                                                                            FROM (SELECT DATE_FORMAT(o.UpdateTime, '{DateFormat}') `Date`,
                                                                                         o.ActualAmount,
                                                                                         o.OrderState
                                                                                  FROM {t} o) t
                                                                            {sqlWhere}
                                                                            GROUP BY t.`Date`
                                                                            """));

        return $"""
                SELECT t.`Date`, SUM(t.OrderCount) SumOrderCount, SUM(t.OrderAmount) SumOrderAmount
                FROM (
                    {unionSql}
                ) t GROUP BY t.`Date`
                ORDER BY t.`Date` DESC
                """;
    }

    private async Task<List<string>> GetExistTables(Guid[] tenantIds)
    {
        var existTables = await dbContext.GetTableNameList(nameof(Order));
        return existTables.Where(x => tenantIds.Any(t => x.Contains(t.ToString().Replace("-", "")))).ToList();
    }

    public virtual async Task<SalesStatisticsList> GetSalesStatList(DateOnly? startDate, DateOnly? endDate, int pageIndex = 1, int pageSize = 10, params Guid[] tenantIds)
    {
        var tableNames = await GetExistTables(tenantIds);
        if (tableNames.Count == 0)
        {
            return new SalesStatisticsList { RecordCount = 0, Items = [] };
        }

        var querySql = SalesStatisticsSql(startDate, endDate, tableNames);

        var query = dbContext.Database.SqlQueryRaw<SalesStatisticsItem>(querySql);
        var count = await query.CountAsync();
        if (count == 0)
        {
            return new SalesStatisticsList { RecordCount = 0, Items = [] };
        }

        var skip = (pageIndex - 1) * pageSize;
        var result = await query.Skip(skip).Take(pageSize).ToListAsync();
        return new SalesStatisticsList { RecordCount = count, Items = result };
    }

    public virtual async Task<SalesStatisticsListDetail> GetSalesStatListDetail(DateOnly startDate, DateOnly endDate, int pageIndex = 1, int pageSize = 10, params Guid[] tenantIds)
    {
        var tableNames = await GetExistTables(tenantIds);
        if (tableNames.Count == 0)
        {
            return new SalesStatisticsListDetail { RecordCount = 0, Items = [] };
        }

        var querySql = SalesStatisticsDetailSql(startDate, endDate, tableNames);

        var query = dbContext.Database.SqlQueryRaw<SalesStatisticsItemDetail>(querySql);
        var count = await query.CountAsync();
        if (count == 0)
        {
            return new SalesStatisticsListDetail { RecordCount = 0, Items = [] };
        }

        var skip = (pageIndex - 1) * pageSize;
        var result = await query.Skip(skip).Take(pageSize).ToListAsync();
        return new SalesStatisticsListDetail { RecordCount = count, Items = result };
    }

    private string SalesStatisticsDetailSql(DateOnly startTime, DateOnly endTime, List<string> tableNames)
    {
        var sqlWhere = BuildWhere(startTime, endTime);

        var unionSql = string.Join("\nUNION ALL\n", tableNames.Select(t => $"""
                                                                            SELECT * FROM (
                                                                              SELECT PlatformPayerName,
                                                                                     OrderSerialNumber,
                                                                                     OrderNumber,
                                                                                     PurchaseName,
                                                                                     OrderAmount,
                                                                                     ActualAmount,
                                                                                     OrderState,
                                                                                     CreatorId,
                                                                                     CreatorName,
                                                                                     CreateTime,
                                                                                     UpdateTime,
                                                                                   DATE_FORMAT(UpdateTime, '{DateFormat}') `Date`
                                                                              FROM {t}
                                                                            ) t
                                                                            {sqlWhere}
                                                                            """));
        return $"SELECT * FROM (\n{unionSql}\n) t ORDER BY t.UpdateTime DESC";
    }
}

public class YearlySalesStatistics(OrderDbContext dbContext) : AbstractSalesStatistics(dbContext), ISalesStatistics
{
    public override SalesStatDimension Dimension => SalesStatDimension.Yearly;

    protected override string DateFormat => "%Y";

    protected override string StartTimeFilter(DateOnly? startTime)
    {
        return startTime.HasValue ? $" AND t.`Date` >= '{startTime.Value.Year}'" : "";
    }

    protected override string EndTimeFilter(DateOnly? endTime)
    {
        return endTime.HasValue ? $" AND t.`Date` < '{endTime.Value.Year}'" : "";
    }
}

public class MonthlySalesStatistics(OrderDbContext dbContext) : AbstractSalesStatistics(dbContext), ISalesStatistics
{
    public override SalesStatDimension Dimension => SalesStatDimension.Monthly;

    protected override string DateFormat => "%Y-%m";

    protected override string StartTimeFilter(DateOnly? startTime)
    {
        return startTime.HasValue ? $" AND t.`Date` >= '{startTime.Value:yyyy-MM}'" : "";
    }

    protected override string EndTimeFilter(DateOnly? endTime)
    {
        return endTime.HasValue ? $" AND t.`Date` < '{endTime.Value:yyyy-MM}'" : "";
    }
}

public class DailySalesStatistics(OrderDbContext dbContext) : AbstractSalesStatistics(dbContext), ISalesStatistics
{
    public override SalesStatDimension Dimension => SalesStatDimension.Daily;

    protected override string DateFormat => "%Y-%m-%d";

    protected override string StartTimeFilter(DateOnly? startTime)
    {
        return startTime.HasValue ? $" AND t.`Date` >= '{startTime:yyyy-MM-dd}'" : "";
    }

    protected override string EndTimeFilter(DateOnly? endTime)
    {
        return endTime.HasValue ? $" AND t.`Date` < '{endTime:yyyy-MM-dd}'" : "";
    }
}