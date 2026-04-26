using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveCouponRepository : IRepository<LiveCoupon>
{
    Task UpdateIsShelvesByIds(ICollection<Guid> ids, bool state);
}
