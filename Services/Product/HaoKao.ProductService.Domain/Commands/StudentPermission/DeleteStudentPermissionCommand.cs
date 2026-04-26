namespace HaoKao.ProductService.Domain.Commands.StudentPermission;

/// <summary>
/// 删除学员权限表命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteStudentPermissionCommand(Guid Id) : Command("删除学员权限表");