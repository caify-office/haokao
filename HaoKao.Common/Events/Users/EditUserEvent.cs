namespace HaoKao.Common.Events.Users;

public record EditUserEvent(
    Guid ExamId,
    string ExamName,
    string UserAccount,
    string UserPassword,
    string UserName,
    string ContactNumber) : IntegrationEvent;