using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;
using ShortUrlService.Domain.Repositories.Base;
using ShortUrlService.WebApi.Configurations;
using System.Threading.Channels;

namespace ShortUrlService.WebApi.Services;

public interface IAccessLogQueue
{
    bool IsCompleted { get; }

    ValueTask EnqueueAsync(AccessLog log, CancellationToken cancellationToken = default);

    ValueTask<AccessLog> DequeueAsync(CancellationToken cancellationToken = default);
}

public class AccessLogQueue : IAccessLogQueue
{
    private readonly Channel<AccessLog> _channel = Channel.CreateUnbounded<AccessLog>();

    public bool IsCompleted => _channel.Reader.Count == 0;

    public ValueTask EnqueueAsync(AccessLog log, CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(log, cancellationToken);
    }

    public ValueTask<AccessLog> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAsync(cancellationToken);
    }
}

public class AccessLogHostedService(
    IAccessLogQueue queue,
    IAccessLogRepository repository,
    IUnitOfWork uow,
    IOptionsSnapshot<ShortUrlConfig> option,
    ILogger<AccessLogHostedService> logger
) : BackgroundService
{
    private readonly SemaphoreSlim _semaphore = new(option.Value.SemaphoreCount);
    private readonly int _bufferSize = option.Value.LogBufferSize;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Queued Hosted Service is running.{NewLine}", Environment.NewLine);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _semaphore.WaitAsync(stoppingToken);

                var buffer = new List<AccessLog>(_bufferSize);
                for (var i = 0; i < _bufferSize; i++)
                {
                    var log = await queue.DequeueAsync(stoppingToken);

                    buffer.Add(log);

                    if (queue.IsCompleted)
                    {
                        logger.LogInformation("The queue is completed. Exiting the loop.{NewLine}", Environment.NewLine);
                        break;
                    }
                }

                logger.LogInformation("DequeueAsync Executed.{NewLine}", Environment.NewLine);

                await repository.AddRangeAsync(buffer);

                await uow.CommitAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Dequeue operation was cancelled.{NewLine}", Environment.NewLine);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing AddMessage.{NewLine}", Environment.NewLine);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}