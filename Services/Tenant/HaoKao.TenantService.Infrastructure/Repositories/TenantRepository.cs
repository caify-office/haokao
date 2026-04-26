using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Repositories;

namespace HaoKao.TenantService.Infrastructure.Repositories;

public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public Task<Tenant> GetByNoAsync(string no)
    {
        return GetAsync(u => u.TenantNo == no);
    }

    public Task<Tenant> GetByNameAsync(string name)
    {
        return GetAsync(u => u.TenantName == name);
    }

    public override async Task<List<Tenant>> GetByQueryAsync(QueryBase<Tenant> query)
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