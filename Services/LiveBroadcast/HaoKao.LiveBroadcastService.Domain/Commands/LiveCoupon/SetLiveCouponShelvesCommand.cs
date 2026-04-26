namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveCoupon;
/// <summary>
/// 修改产品上下架
/// </summary>
/// <param name="Ids"></param>
/// <param name="IsShelves">产品上下架</param>
public record SetLiveCouponShelvesCommand(ICollection<Guid> Ids, bool IsShelves) : Command("修改优惠卷上下架");