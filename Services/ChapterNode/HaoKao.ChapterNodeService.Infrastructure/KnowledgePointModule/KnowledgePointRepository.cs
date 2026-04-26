using Girvs.BusinessBasis.Queries;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.ChapterNodeService.Infrastructure.KnowledgePointModule;

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
                               .OrderBy(x => x.Sort)
                               .ThenBy(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}