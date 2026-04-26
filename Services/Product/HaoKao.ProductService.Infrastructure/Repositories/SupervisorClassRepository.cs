using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Infrastructure.Repositories;

public class SupervisorClassRepository : Repository<SupervisorClass>, ISupervisorClassRepository
{
    public override async Task<List<SupervisorClass>> GetByQueryAsync(QueryBase<SupervisorClass> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).Include(x => x.SupervisorStudents).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .Include(x => x.SupervisorStudents)
                               .SelectProperties(query.QueryFields)
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}