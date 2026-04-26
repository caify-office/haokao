using HaoKao.BasicService.Domain.Events;
using HaoKao.Common.Events.Authorize;

namespace HaoKao.BasicService.Domain.EventHandlers;

public class RemoveServiceCacheEventHandler(IEventBus eventBus) : INotificationHandler<RemoveServiceCacheEvent>
{
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

    public async Task Handle(RemoveServiceCacheEvent notification, CancellationToken cancellationToken)
    {
        var @event = new RemoveAuthorizeCacheEvent();
        await _eventBus.PublishAsync(@event);
    }
}