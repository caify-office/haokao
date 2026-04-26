using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.SubjectService.Domain.SubjectModule;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.SubjectService.Infrastructure.Repositories;

public class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public override async Task<List<Subject>> GetByQueryAsync(QueryBase<Subject> query)
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
                               .Take(query.PageSize)
                               .ToListAsync();
        }

        return query.Result;
    }
}