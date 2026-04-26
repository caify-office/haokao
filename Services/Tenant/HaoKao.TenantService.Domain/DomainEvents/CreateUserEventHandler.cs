using HaoKao.Common.Events.Users;

namespace HaoKao.TenantService.Domain.DomainEvents;

public class CreateUserEventHandler(IEventBus eventBus) : INotificationHandler<CreateUserDomainEvent>
{
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

    public async Task Handle(CreateUserDomainEvent notification, CancellationToken cancellationToken)
    {
        var createUser = new CreateUserEvent(
            notification.UserAccount,
            notification.UserPassword,
            notification.UserName,
            notification.ContactNumber,
            notification.UserType,
            notification.TenantId,
            notification.TenantName
        );
        await _eventBus.PublishAsync(createUser);
    }
}