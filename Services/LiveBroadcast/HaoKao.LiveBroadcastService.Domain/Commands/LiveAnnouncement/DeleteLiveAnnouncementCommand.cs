namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveAnnouncement;

/// <summary>
/// 删除直播公告命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLiveAnnouncementCommand(
    Guid Id
) : Command("删除直播公告");