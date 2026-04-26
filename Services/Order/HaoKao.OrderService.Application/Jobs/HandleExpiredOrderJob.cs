using Girvs.Quartz;
using HaoKao.Common.Events.Coupon;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HaoKao.OrderService.Application.Jobs;

public class HandleExpiredOrderJob(IServiceProvider serviceProvider) : GirvsJob(serviceProvider)
{
    public override void GirvsExecute(IJobExecutionContext context)
    {
        var repository = EngineContext.Current.Resolve<IOrderRepository>();
        var eventBus = EngineContext.Current.Resolve<IEventBus>();
        var tasks = new List<Task>();

        try
        {
            // 查询出失效订单
            var orders = repository.QueryAllExpiredOrderList().Result;
            foreach (var groupingOrder in orders.GroupBy(x => x.TenantId))
            {
                EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
                {
                    {
                        GirvsIdentityClaimTypes.TenantId, groupingOrder.Key.ToString()
                    }
                });
                tasks.Add(repository.SetOrderExpired(groupingOrder.Select(x => x.Id)));
                tasks.AddRange(groupingOrder.Select(order => eventBus.PublishAsync(new UpdateIsLockedEvent(order.Id))));
                Task.WaitAll(tasks.ToArray());
                tasks.Clear();
            }
        }
        catch (Exception e)
        {
            EngineContext.Current.Resolve<ILogger<Order>>().Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }
}