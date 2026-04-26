namespace HaoKao.Common.Events.StudentPermission;

/// <summary>
/// 更新学员权限的过期时间
/// </summary>
/// <param name="StudentId">学员Id</param>
/// <param name="ProductId">产品Id</param>
/// <param name="ExpiryTime">过期时间</param>
public record UpdateStudentPermissionExpiryTimeEvent(Guid StudentId, Guid ProductId, DateTime ExpiryTime) : IntegrationEvent;