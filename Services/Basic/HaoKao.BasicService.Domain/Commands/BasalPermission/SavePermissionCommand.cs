namespace HaoKao.BasicService.Domain.Commands.BasalPermission;

/// <summary>
/// 保存操作权限
/// </summary>
/// <param name="AppliedId">对应用户ID或角色ID</param>
/// <param name="AppliedObjectType">对应功能模块的ID</param>
/// <param name="ValidateObjectType">权限分类</param>
/// <param name="ObjectPermissions">权限数据</param>
public record SavePermissionCommand(
    Guid AppliedId,
    PermissionAppliedObjectType AppliedObjectType,
    PermissionValidateObjectType ValidateObjectType,
    IList<ObjectPermission> ObjectPermissions
) : Command("保存操作权限");

public class ObjectPermission
{
    /// <summary>
    /// 对应功能模块的ID
    /// </summary>
    public Guid AppliedObjectId { get; set; }

    /// <summary>
    /// 权限值转换具体说明集合
    /// </summary>
    public List<Permission> PermissionOperation { get; set; } = [];
}