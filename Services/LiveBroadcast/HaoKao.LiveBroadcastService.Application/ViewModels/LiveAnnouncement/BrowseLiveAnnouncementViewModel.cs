namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveAnnouncement;

[AutoMapFrom(typeof(Domain.Entities.LiveAnnouncement))]
public class BrowseLiveAnnouncementViewModel : IDto
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}