namespace HaoKao.Common.Events.Student;

public record UpdateStudentPaidEvent(Guid RegisterUserId) : IntegrationEvent;