namespace HaoKao.TenantService.Domain.DomainEvents;

public record EditUserDomainEvent(
    Guid TenantId,
    string TenantName,
    string UserAccount,
    string UserPassword,
    string UserName,
    string ContactNumber
) : Event;