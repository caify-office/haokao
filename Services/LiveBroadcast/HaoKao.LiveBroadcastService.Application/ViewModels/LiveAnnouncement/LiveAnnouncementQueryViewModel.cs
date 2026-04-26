namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveAnnouncement;

[AutoMapFrom(typeof(LiveAnnouncementQuery))]
[AutoMapTo(typeof(LiveAnnouncementQuery))]
public class LiveAnnouncementQueryViewModel : QueryDtoBase<LiveAnnouncementQueryListViewModel>
{
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.LiveAnnouncement))]
[AutoMapTo(typeof(Domain.Entities.LiveAnnouncement))]
public class LiveAnnouncementQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}