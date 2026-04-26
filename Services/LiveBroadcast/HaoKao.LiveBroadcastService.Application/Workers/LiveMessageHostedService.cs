using Girvs.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace HaoKao.LiveBroadcastService.Application.Workers;

public class LiveMessageHostedService(ILiveMessageQueue queue, ILogger<LiveMessageHostedService> logger) : BackgroundService
{
    private readonly SemaphoreSlim _semaphore = new(100);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Queued Hosted Service is running.{NewLine}", Environment.NewLine);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _semaphore.WaitAsync(stoppingToken);

                var message = await queue.DequeueAsync(stoppingToken);

                logger.LogInformation("DequeueAsync Executed.{NewLine}", Environment.NewLine);

                EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
                {
                    { GirvsIdentityClaimTypes.UserId, message.CreatorId.ToString() },
                    { GirvsIdentityClaimTypes.UserName, message.CreatorName },
                    { GirvsIdentityClaimTypes.TenantId, message.TenantId.ToString() },
                });
                var repository = EngineContext.Current.Resolve<ILiveMessageRepository>();
                await repository.AddMessage(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing AddMessage");
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}