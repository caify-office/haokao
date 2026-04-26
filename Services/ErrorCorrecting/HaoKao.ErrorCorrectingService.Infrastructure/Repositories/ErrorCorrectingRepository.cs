using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using HaoKao.ErrorCorrectingService.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.ErrorCorrectingService.Infrastructure.Repositories;

public class ErrorCorrectingRepository : Repository<ErrorCorrecting>, IErrorCorrectingRepository
{
    public override async Task<List<ErrorCorrecting>> GetByQueryAsync(QueryBase<ErrorCorrecting> query)
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