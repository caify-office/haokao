using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveReservation;

/// <summary>
/// 创建直播预约命令
/// </summary>
public record CreateLiveReservationCommand(
    Guid ProductId,
    Guid LiveVideoId,
    string Phone,
    ReservationSource ReservationSource,
    string OpenId
) : Command("创建直播预约命令");