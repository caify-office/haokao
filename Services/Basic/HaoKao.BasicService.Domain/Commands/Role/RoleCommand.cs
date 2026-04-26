namespace HaoKao.BasicService.Domain.Commands.Role;

/// <summary>
/// 角色命令
/// </summary>
/// <param name="Name">角色名称</param>
/// <param name="Desc">描述</param>
/// <param name="UserIds">用户Id集合</param>
/// <param name="CommandDesc">命令描述</param>
public abstract record RoleCommand(string Name, string Desc, Guid[] UserIds, string CommandDesc) : Command(CommandDesc);