

using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.BurialPointService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.BurialPointService.Infrastructure.Repositories;

public class BrowseRecordRepository : Repository<BrowseRecord>, IBrowseRecordRepository
{
    public override async Task<List<BrowseRecord>> GetByQueryAsync(QueryBase<BrowseRecord> query)
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
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}
