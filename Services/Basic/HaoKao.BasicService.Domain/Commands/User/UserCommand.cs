namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 用户命令
/// </summary>
/// <param name="UserAccount">用户账户</param>
/// <param name="UserName">用户名称</param>
/// <param name="ContactNumber">用户联系方式</param>
/// <param name="UserPassword">用户密码</param>
/// <param name="CommandDesc">命令描述</param>
public abstract record UserCommand(
    string UserAccount,
    string UserName,
    string ContactNumber,
    string UserPassword,
    string CommandDesc
) : Command(CommandDesc);