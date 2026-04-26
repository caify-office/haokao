namespace HaoKao.Common.Events.StudentPermission;

public record CreateStudentPermissionEvent(
    string StudentName,
    Guid StudentId,
    string OrderNumber,
    Guid ProductId,
    string ProductName,
    string PurchaseProductContents,
    int SourceMode
) : IntegrationEvent;