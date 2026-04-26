namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveVideo;

/// <summary>
/// 删除视频直播命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLiveVideoCommand(
    Guid Id
) : Command("删除视频直播");