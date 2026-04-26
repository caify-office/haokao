using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;

[AutoMapFrom(typeof(Domain.Entities.LiveReservation))]
public class BrowseLiveReservationViewModel : IDto
{
    /// <summary>
    /// 预约直播产品id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 预约视频直播Id
    /// </summary>
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 预约来源
    /// </summary>
    public ReservationSource ReservationSource { get; set; }

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string CreatorName { get; set; }
}