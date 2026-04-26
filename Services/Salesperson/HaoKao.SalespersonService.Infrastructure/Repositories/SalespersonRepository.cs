using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Queries;
using HaoKao.SalespersonService.Domain.Repositories;

namespace HaoKao.SalespersonService.Infrastructure.Repositories;

public class SalespersonRepository : Repository<Salesperson>, ISalespersonRepository
{
    public Task<Salesperson> GetWithConfigById(Guid id)
    {
        return Queryable.AsNoTracking().Include(x => x.EnterpriseWeChatConfig).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SalespersonQuery> GetWithConfigByQuery(SalespersonQuery query)
    {
        query.RecordCount = await Queryable.AsNoTracking().CountAsync(query.GetQueryWhere());
        if (query.RecordCount == 0)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.AsNoTracking()
                                          .Include(x => x.EnterpriseWeChatConfig)
                                          .Where(query.GetQueryWhere())
                                          .OrderByDescending(x => x.CreateTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize)
                                          .ToListAsync();
        }
        return query;
    }

    public async Task<IReadOnlyList<Salesperson>> GetIncludeAll()
    {
        return await Queryable.AsNoTracking()
                              .Include(x => x.EnterpriseWeChatConfig)
                              .Where(x => !string.IsNullOrEmpty(x.EnterpriseWeChatUserId))
                              .ToListAsync();
    }

    public Task DeleteByIds(IReadOnlyList<Guid> ids)
    {
        return Queryable.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
    }
}