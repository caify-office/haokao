using HaoKao.LiveBroadcastService.Domain.Commands.LiveAnnouncement;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveAnnouncement;

[AutoMapTo(typeof(UpdateLiveAnnouncementCommand))]
public class UpdateLiveAnnouncementViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [DisplayName("标题")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [DisplayName("内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Content { get; set; }
}