namespace HaoKao.Common.Events.Student;

public record CreateStudentEvent(Guid RegisterUserId, Guid TenantId) : IntegrationEvent;