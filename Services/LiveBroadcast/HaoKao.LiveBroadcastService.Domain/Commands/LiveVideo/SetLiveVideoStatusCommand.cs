using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveVideo;

/// <summary>
/// 更新视频直播命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="LiveStatus">直播状态</param>
public record SetLiveVideoStatusCommand(
    Guid Id,
    LiveStatus LiveStatus
) : Command("更新视频直播状态");