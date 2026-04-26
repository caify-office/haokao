using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LiveOnlineUserRepository : Repository<LiveOnlineUser>, ILiveOnlineUserRepository
{
    public IQueryable<LiveOnlineUser> Query => Queryable.AsNoTracking();

    public Task<LiveOnlineUser> GetUserByLiveId(Guid liveId)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return Query.FirstOrDefaultAsync(x => x.LiveId == liveId && x.CreatorId == userId);
    }
}