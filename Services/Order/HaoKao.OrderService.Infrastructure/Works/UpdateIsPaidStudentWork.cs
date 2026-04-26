using Girvs.EventBus;
using HaoKao.Common.Events.Student;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Works;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.OrderService.Infrastructure.Works;

public class UpdateIsPaidOrderWork(IServiceProvider provider) : IUpdateIsPaidOrderWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(Order));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<OrderDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var orders = await tenantDbContext.Orders.Where(x => x.ActualAmount >= 200 && x.OrderState == OrderState.PaymentSuccessful).ToListAsync();
            await Task.WhenAll(orders.Select(x => eventBus.PublishAsync(new UpdateStudentPaidEvent(x.CreatorId))));
        }
    }
}