namespace HaoKao.Common.Events.Users;

public record CreateUserEvent(
    string UserAccount,
    string UserPassword,
    string UserName,
    string ContactNumber,
    UserType UserType,
    Guid TenantId,
    string TenantName) : IntegrationEvent;