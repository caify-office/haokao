using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveProductPackageRepository : IRepository<LiveProductPackage>
{
    Task UpdateIsShelvesByIds(ICollection<Guid> ids, bool state);
}