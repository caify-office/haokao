using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveVideoRepository : IRepository<LiveVideo>
{
    IQueryable<LiveVideo> Query { get; }

    Task<List<LiveVideo>> GetByQueryOrderByAsync(QueryBase<LiveVideo> query);
}