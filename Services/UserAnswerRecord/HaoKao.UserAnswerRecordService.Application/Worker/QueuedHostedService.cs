using HaoKao.UserAnswerRecordService.Domain.Extensions;
using Microsoft.Extensions.Hosting;

namespace HaoKao.UserAnswerRecordService.Application.Worker;

public class QueuedHostedService(
    ILogger<QueuedHostedService> logger,
    IStaticCacheManager cacheManager,
    IBackgroundTaskQueue taskQueue)
    : BackgroundService
{
    public IBackgroundTaskQueue TaskQueue { get; } = taskQueue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Queued Hosted Service is running.{NewLine}", Environment.NewLine);

        // 服务重启, 重置所有任务的刷新状态
        cacheManager.RemoveByPrefix(new UserAnswerCacheDefaults().RefreshPrefix.Key);

        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await TaskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing {WorkItem}", nameof(workItem));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Queued Hosted Service is stopping");

        await base.StopAsync(stoppingToken);
    }
}