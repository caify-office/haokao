using Girvs.BusinessBasis.Queries;
using HaoKao.KnowledgePointService.Domain.Entities;
using HaoKao.KnowledgePointService.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.KnowledgePointService.Infrastructure.Repositories;

public class KnowledgePointRepository : Repository<KnowledgePoint>, IKnowledgePointRepository 
{
    public override async Task<List<KnowledgePoint>> GetByQueryAsync(QueryBase<KnowledgePoint> query)
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
                               //.SelectProperties(query.QueryFields)
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}