namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveCoupon;
/// <summary>
/// 删除直播优惠卷命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLiveCouponCommand(
    Guid Id
) : Command("删除直播优惠卷");