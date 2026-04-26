using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILivePlayBackRepository : IRepository<LivePlayBack> 
{
    Task<LivePlayBack> GetIncludeLiveVideo(Guid id);
}