namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 更新用户角色
/// </summary>
/// <param name="Id">用户Id</param>
/// <param name="RoleIds">角色Id集合</param>
public record UpdateUserRoleCommand(Guid Id, Guid[] RoleIds) : Command("更新用户角色");