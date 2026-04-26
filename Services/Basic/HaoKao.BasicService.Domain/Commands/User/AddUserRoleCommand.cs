namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 添加用户角色
/// </summary>
/// <param name="UserId">用户Id</param>
/// <param name="RoleIds">角色Id集合</param>
public record AddUserRoleCommand(Guid UserId, IList<Guid> RoleIds) : Command("添加用户角色");