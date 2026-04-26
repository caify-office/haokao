using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.LecturerService.Domain.Entities;
using HaoKao.LecturerService.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LecturerService.Infrastructure.Repositories;

public class LecturerRepository : Repository<Lecturer>, ILecturerRepository
{
    public override async Task<List<Lecturer>> GetByQueryAsync(QueryBase<Lecturer> query)
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