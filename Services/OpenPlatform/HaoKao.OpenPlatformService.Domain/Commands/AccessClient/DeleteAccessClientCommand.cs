namespace HaoKao.OpenPlatformService.Domain.Commands.AccessClient;

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteAccessClientCommand(
    Guid Id
) : Command("删除");