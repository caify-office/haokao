using HaoKao.Common.Events.Users;

namespace HaoKao.TenantService.Domain.DomainEvents;

public class EditUserEventHandler(IEventBus eventBus) : INotificationHandler<EditUserDomainEvent>
{
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

    public async Task Handle(EditUserDomainEvent notification, CancellationToken cancellationToken)
    {
        var createUser = new EditUserEvent(
            notification.TenantId,
            notification.TenantName,
            notification.UserAccount,
            notification.UserPassword,
            notification.UserName,
            notification.ContactNumber
        );
        await _eventBus.PublishAsync(createUser);
    }
}