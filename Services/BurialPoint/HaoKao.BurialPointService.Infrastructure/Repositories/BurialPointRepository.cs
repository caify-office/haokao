using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.BurialPointService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.BurialPointService.Infrastructure.Repositories;

public class BurialPointRepository : Repository<BurialPoint>, IBurialPointRepository
{
    public override async Task<List<BurialPoint>> GetByQueryAsync(QueryBase<BurialPoint> query)
    {
        query.RecordCount = await Queryable.Include(x=>x.BrowseRecords).Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Include(x => x.BrowseRecords).Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize)
                               .ToListAsync();
        }

        return query.Result;
    }
    public override Task<BurialPoint> GetByIdAsync(Guid id)
    {
        return Queryable.Include(x=>x.BrowseRecords).FirstOrDefaultAsync( t => t.Id.Equals(id));
    }
}
