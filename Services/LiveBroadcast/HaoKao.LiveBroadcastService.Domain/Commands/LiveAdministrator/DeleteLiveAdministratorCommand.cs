namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveAdministrator;

/// <summary>
/// 删除直播管理员命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLiveAdministratorCommand(
    Guid Id
) : Command("删除直播管理员");