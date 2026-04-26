using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveOnlineUserRepository : IRepository<LiveOnlineUser>
{
    IQueryable<LiveOnlineUser> Query { get; }

    Task<LiveOnlineUser> GetUserByLiveId(Guid liveId);
}