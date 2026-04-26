using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LivePlayBackRepository : Repository<LivePlayBack>, ILivePlayBackRepository
{
    public override async Task<List<LivePlayBack>> GetByQueryAsync(QueryBase<LivePlayBack> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderBy(x => x.Sort)
                               .ThenByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public  async Task<LivePlayBack> GetIncludeLiveVideo(Guid id)
    {
        return await Queryable.Include(x => x.LiveVideo).FirstAsync(x => x.Id == id);
    }
}