using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.Repositories;

public class LearningTaskRepository(LearningPlanDbContext dbContext) : Repository<LearningTask>, ILearningTaskRepository
{
    public override async Task<List<LearningTask>> GetByQueryAsync(QueryBase<LearningTask> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable
                                 .Where(query.GetQueryWhere())
                                 .SelectProperties(query.QueryFields)
                                 .OrderBy(x => x.Sort)
                                 .Skip(query.PageStart)
                                 .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public Task<List<LearningTask>> GetWhere(Expression<Func<LearningTask, bool>> expression)
    {
       return dbContext.LearningTasks.Where(expression).ToListAsync();
    }

}
