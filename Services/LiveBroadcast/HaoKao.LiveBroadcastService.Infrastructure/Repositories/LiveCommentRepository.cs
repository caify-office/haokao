using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LiveCommentRepository : Repository<LiveComment>, ILiveCommentRepository
{
    /// <summary>
    /// 综合评分
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    public Task<double> GetAverageRating(Guid liveId)
    {
        return Queryable.GroupBy(x => x.LiveId)
                        .Where(x => x.Key == liveId)
                        .Select(x => 1.0 * x.Sum(i => i.Rating) / x.Count())
                        .FirstOrDefaultAsync();
    }
}