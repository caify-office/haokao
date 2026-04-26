using Girvs.Quartz;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace HaoKao.OpenPlatformService.Application.Jobs;

public class CleanExpiredGrantJob(IServiceProvider serviceProvider) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override void GirvsExecute(IJobExecutionContext context)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IPersistedGrantRepository>();
        repository.ClearExpiredAsync().Wait();
    }
}