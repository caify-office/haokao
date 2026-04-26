namespace HaoKao.ProductService.Domain.Commands.StudentPermission;

/// <summary>
/// 更新学员权限表命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Enable">启用/禁用</param>
public record UpdateStudentPermissionStateCommand(Guid Id, bool Enable) : Command("更新学员权限表");

/// <summary>
/// 更新学员权限的过期时间
/// </summary>
/// <param name="Id">Id</param>
/// <param name="ExpiryTime">过期时间</param>
public record UpdateStudentPermissionExpiryTimeCommand(Guid Id, DateTime ExpiryTime) : Command("更新学员权限的过期时间");

/// <summary>
/// 更新学员权限的过期时间
/// </summary>
/// <param name="StudentId">学员Id</param>
/// <param name="ProductId">产品Id</param>
/// <param name="ExpiryTime">过期时间</param>
public record UpdateStudentPermissionExpiryTimeEventCommand(Guid StudentId, Guid ProductId, DateTime ExpiryTime) : Command("更新学员权限的过期时间");