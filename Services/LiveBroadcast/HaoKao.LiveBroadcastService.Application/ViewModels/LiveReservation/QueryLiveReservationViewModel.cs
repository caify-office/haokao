using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;

[AutoMapFrom(typeof(LiveReservationQuery))]
[AutoMapTo(typeof(LiveReservationQuery))]
public class QueryLiveReservationViewModel : QueryDtoBase<BrowseLiveReservationViewModel>
{
    /// <summary>
    /// 预约直播产品id
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 预约视频直播Id
    /// </summary>
    public Guid? LiveVideoId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 预约来源
    /// </summary>
    public ReservationSource ReservationSource { get; set; }
}