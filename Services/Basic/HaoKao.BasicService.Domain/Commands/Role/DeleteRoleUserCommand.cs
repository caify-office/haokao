namespace HaoKao.BasicService.Domain.Commands.Role;

/// <summary>
/// 删除角色用户
/// </summary>
/// <param name="RoleId">角色Id</param>
/// <param name="UserIds">用户Id集合</param>
public record DeleteRoleUserCommand(Guid RoleId, IList<Guid> UserIds) : Command("删除角色用户");