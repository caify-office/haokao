using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Threading;
using System.Threading.Channels;

namespace HaoKao.LiveBroadcastService.Application.Workers;

public interface ILiveMessageQueue
{
    bool IsCompleted { get; }

    ValueTask EnqueueAsync(LiveMessage message, CancellationToken cancellationToken = default);

    ValueTask<LiveMessage> DequeueAsync(CancellationToken cancellationToken = default);
}

public class LiveMessageQueue : ILiveMessageQueue
{
    private readonly Channel<LiveMessage> _channel = Channel.CreateUnbounded<LiveMessage>();

    public bool IsCompleted => _channel.Reader.Completion.IsCompleted;

    public ValueTask EnqueueAsync(LiveMessage message, CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(message, cancellationToken);
    }

    public ValueTask<LiveMessage> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAsync(cancellationToken);
    }
}