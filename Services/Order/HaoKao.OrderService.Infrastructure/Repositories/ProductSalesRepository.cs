using Girvs.BusinessBasis.Repositories;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Queries;
using HaoKao.OrderService.Domain.Repositories;
using HaoKao.OrderService.Domain.ValueObjects;

namespace HaoKao.OrderService.Infrastructure.Repositories;

public class ProductSalesRepository(OrderDbContext dbContext) : Repository<ProductSales>, IProductSalesRepository
{
    public async Task<ProductSalesStatList> GetProductSalesStatListAsync(ProducSalesQuery query)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<ProductSales>().GetCurrentShardingTableName();
        var currentMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM"));
        var nextMonth = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("yyyy-MM"));
        var sql = $"""
            SELECT
                `p`.`Id` as `ProductId`,
                `p`.`Name` as `ProductName`,
                IFNULL(`ps`.`SalesCount`, 0) as `SalesCount`,
                IFNULL(`ps`.`SalesRevenue`, 0) as `SalesRevenue`,
                `p`.`TenantId`
            FROM `Product` `p`
            LEFT JOIN (
                SELECT `p`.`ProductId`, COUNT(*) AS `SalesCount`, COALESCE(SUM(`p`.`ActualPrice`), 0.0) AS `SalesRevenue`
                FROM `{tableName}` AS `p`
                WHERE 1 = 1 
                  AND `p`.`CreateTime` >= '{(query.StartTime.HasValue ? $"{query.StartTime.Value:yyyy-MM-dd HH:mm:ss}" : $"{currentMonth:yyyy-MM-dd HH:mm:ss}")}'
                  AND `p`.`CreateTime` < '{(query.EndTime.HasValue ? $"{query.EndTime.Value:yyyy-MM-dd HH:mm:ss}" : $"{nextMonth:yyyy-MM-dd HH:mm:ss}")}'
                GROUP BY `p`.`ProductId`
            ) `ps` ON `p`.`Id` = `ps`.`ProductId`
            """;
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().ToGuid();
        var linq = dbContext.Database.SqlQueryRaw<ProductSalesStat>(sql).Where(x => x.TenantId == tenantId);

        var count = await linq.CountAsync();
        if (count == 0) return new(0, []);

        linq = query.OrderBy.Equals("salesrevenue", StringComparison.OrdinalIgnoreCase)
            ? linq.OrderByDescending(x => x.SalesRevenue)
            : linq.OrderByDescending(x => x.SalesCount);

        var result = await linq.Skip(query.PageStart).Take(query.PageSize).ToListAsync();
        return new(count, result);
    }
}