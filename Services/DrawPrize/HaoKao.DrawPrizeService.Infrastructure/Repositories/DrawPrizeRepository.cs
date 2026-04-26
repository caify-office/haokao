using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.DrawPrizeService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.DrawPrizeService.Infrastructure.Repositories;

public class DrawPrizeRepository : Repository<DrawPrize>, IDrawPrizeRepository
{
    public Task UpdateEnableByIds(ICollection<Guid> ids, bool state)
    {
        return Queryable.Where(x => ids.Contains(x.Id)).ExecuteUpdateAsync(
            s => s.SetProperty(x => x.Enable, state)
        );
    }

    public override async Task<List<DrawPrize>> GetByQueryAsync(QueryBase<DrawPrize> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable
                .Include(x => x.Prizes.OrderByDescending(x => x.CreateTime))
                .Include(x => x.DrawPrizeRecords.OrderByDescending(x => x.CreateTime))
                .Where(query.GetQueryWhere())
                .SelectProperties(query.QueryFields)
                .OrderByDescending(x => x.CreateTime)
                .Skip(query.PageStart)
                .Take(query.PageSize)
                .ToListAsync();
        }

        return query.Result;
    }

    public override async Task<DrawPrize> GetByIdAsync(Guid id)
    {
        var result = await Queryable
            .Include(x => x.Prizes.OrderByDescending(x => x.CreateTime))
            .Include(x => x.DrawPrizeRecords.OrderByDescending(x => x.CreateTime))
            .FirstAsync(x => x.Id == id);
        return result;
    }

    public Task DeleteByIds(IReadOnlyList<Guid> ids)
    {
        return Queryable.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
    }
}
