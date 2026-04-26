using Girvs.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

// ReSharper disable UnusedMember.Global

namespace HaoKao.LiveBroadcastService.Application.Jobs;

public class Track2MinOnlineUserTrendJob(IServiceProvider serviceProvider, ILogger<Track2MinOnlineUserTrendJob> logger) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override void GirvsExecute(IJobExecutionContext context)
    {
        try
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<ILiveOnlineUserTrendRepository>();
            repository.TrackOnlineUserTrend(2).GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}

public class Track5MinOnlineUserTrendJob(IServiceProvider serviceProvider, ILogger<Track5MinOnlineUserTrendJob> logger) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override void GirvsExecute(IJobExecutionContext context)
    {
        try
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<ILiveOnlineUserTrendRepository>();
            repository.TrackOnlineUserTrend(5).GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}

public class Track10MinOnlineUserTrendJob(IServiceProvider serviceProvider, ILogger<Track10MinOnlineUserTrendJob> logger) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override void GirvsExecute(IJobExecutionContext context)
    {
        try
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<ILiveOnlineUserTrendRepository>();
            repository.TrackOnlineUserTrend(10).GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}