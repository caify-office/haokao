namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 删除用户
/// </summary>
/// <param name="Id">用户Id</param>
public record DeleteUserCommand(Guid Id) : Command("删除用户");