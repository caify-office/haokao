using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.OrderService.Infrastructure.Repositories;

public class OrderRepository(
    OrderDbContext dbContext,
    ILogger<OrderRepository> logger
) : Repository<Order>, IOrderRepository
{
    public override async Task<List<Order>> GetByQueryAsync(QueryBase<Order> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable
                                 .Include(x => x.OrderInvoice)
                                 .Where(query.GetQueryWhere())
                                 .SelectProperties(query.QueryFields)
                                 .OrderByDescending(x => x.CreateTime)
                                 .Skip(query.PageStart)
                                 .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public override async Task<Order> GetByIdAsync(Guid id)
    {
        return await dbContext.Orders.FindAsync(id);
    }

    public bool Update(Order order)
    {
        dbContext.Orders.Update(order);
        var changeRowCount = dbContext.SaveChanges();
        logger.LogInformation($"提交保存时的DBContextId为：{dbContext.ContextId.InstanceId.ToString()},保存时影响的行数为：{changeRowCount}");
        return changeRowCount > 0;
    }

    public async Task<List<Order>> QueryAllExpiredOrderList()
    {
        var database = dbContext.Database;
        var conn = database.GetDbConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "Sp_QueryAllExpiredOrder";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.AddParameter("SchemaName", conn.Database);

        var orders = new List<Order>();
        await database.OpenConnectionAsync();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            orders.Add(new Order
            {
                Id = reader["Id"].To<Guid>(),
                TenantId = reader["TenantId"].To<Guid>(),
            });
        }
        await database.CloseConnectionAsync();
        return orders;
    }

    public async Task SetOrderExpired(IEnumerable<Guid> ids)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<Order>().GetCurrentShardingTableName();
        var sql = $"UPDATE {tableName} SET `OrderState` = {(int)OrderState.Expired} WHERE `Id` IN ({string.Join(", ", ids.Select(x => $"'{x}'"))})";
        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    public Task<decimal> GetLiveTotalAmount(Guid liveId)
    {
        return Queryable.Where(x => x.LiveId == liveId && x.OrderState == OrderState.PaymentSuccessful)
                        .SumAsync(x => x.ActualAmount);
    }

    public Task<int> GetLiveTransactionPeopleNumber(Guid liveId)
    {
        return Queryable.Where(x => x.LiveId == liveId && x.OrderState == OrderState.PaymentSuccessful)
                        .GroupBy(x => x.CreatorId)
                        .CountAsync();
    }

    public async Task InitProductSales()
    {
        var provider = EngineContext.Current.Resolve<IServiceProvider>();
        await using var scope = provider.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        var tables = await context.GetTableNameList(nameof(Order));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<OrderDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var orders = await tenantDbContext.Orders.AsNoTracking().Where(x => x.OrderState == OrderState.PaymentSuccessful).ToListAsync();
            var productSales = orders.SelectMany(x => x.PurchaseProductContents.Select(y => new ProductSales
            {
                ProductId = y.ContentId,
                ProductName = y.ContentName,
                ActualPrice = y.ActualAmount,
                CreateTime = x.UpdateTime
            })).ToList();
            await tenantDbContext.ProductSales.AddRangeAsync(productSales);
            await tenantDbContext.SaveChangesAsync();
        }
    }

    public async Task AddProductSales(Order order)
    {
        var productSales = order.PurchaseProductContents.Select(y => new ProductSales
        {
            ProductId = y.ContentId,
            ProductName = y.ContentName,
            ActualPrice = y.ActualAmount,
            CreateTime = order.UpdateTime
        }).ToList();
        await dbContext.ProductSales.AddRangeAsync(productSales);
        await dbContext.SaveChangesAsync();
    }
}