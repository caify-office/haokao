namespace HaoKao.TenantService.Domain.DomainEvents;

public record CreateUserDomainEvent(
    string UserAccount,
    string UserPassword,
    string UserName,
    string ContactNumber,
    UserType UserType,
    Guid TenantId,
    string TenantName
) : Event;