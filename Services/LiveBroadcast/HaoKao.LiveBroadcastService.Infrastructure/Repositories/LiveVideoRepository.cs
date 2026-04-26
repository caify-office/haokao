using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LiveVideoRepository : Repository<LiveVideo>, ILiveVideoRepository
{
    public IQueryable<LiveVideo> Query => Queryable;

    public override async Task<List<LiveVideo>> GetByQueryAsync(QueryBase<LiveVideo> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.Where(query.GetQueryWhere())
                                          .SelectProperties(query.QueryFields)
                                          .OrderByDescending(x => x.StartTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize)
                                          .ToListAsync();
        }

        return query.Result;
    }

    public async Task<List<LiveVideo>> GetByQueryOrderByAsync(QueryBase<LiveVideo> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.Where(query.GetQueryWhere())
                                          .SelectProperties(query.QueryFields)
                                          .OrderBy(x => x.StartTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize)
                                          .ToListAsync();
        }

        return query.Result;
    }
}