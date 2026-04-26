namespace HaoKao.BasicService.Domain.Commands.Role;

/// <summary>
/// 删除角色
/// </summary>
/// <param name="Id">角色Id</param>
public record DeleteRoleCommand(Guid Id) : Command("删除角色");