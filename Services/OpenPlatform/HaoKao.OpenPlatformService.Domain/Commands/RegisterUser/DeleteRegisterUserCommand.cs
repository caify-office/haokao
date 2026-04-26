namespace HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;

/// <summary>
/// 删除注册用户命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteRegisterUserCommand(
    Guid Id
) : Command("删除注册用户");