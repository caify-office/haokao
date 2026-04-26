namespace HaoKao.LiveBroadcastService.Domain.Commands.LivePlayBack;

/// <summary>
/// 删除直播回放命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLivePlayBackCommand(
    Guid Id
) : Command("删除直播回放");