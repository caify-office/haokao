using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveVideo;

[AutoMapTo(typeof(SetLiveVideoStatusCommand))]
public class SetLiveVideoStatusViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 直播状态
    /// </summary>
    [DisplayName("直播状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public LiveStatus LiveStatus { get; set; }
}