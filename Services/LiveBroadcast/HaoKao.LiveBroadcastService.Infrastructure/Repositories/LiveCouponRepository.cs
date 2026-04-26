using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LiveCouponRepository : Repository<LiveCoupon>, ILiveCouponRepository
{
    public override async Task<List<LiveCoupon>> GetByQueryAsync(QueryBase<LiveCoupon> query)
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

    public Task UpdateIsShelvesByIds(ICollection<Guid> ids, bool state)
    {
        return Queryable.Where(x => ids.Contains(x.Id)).ExecuteUpdateAsync(
            s => s.SetProperty(x => x.IsShelves, state)
        );
    }
}