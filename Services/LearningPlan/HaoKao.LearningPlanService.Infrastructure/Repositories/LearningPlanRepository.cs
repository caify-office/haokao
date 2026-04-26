

using HaoKao.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.Repositories;

public class LearningPlanRepository(LearningPlanDbContext dbContext) : Repository<LearningPlan>, ILearningPlanRepository
{
    public Task<LearningPlan> GetIncludeByCreatorIdAsync( Guid creatorId, Guid subjectId,Guid productId)
    {
      return  Queryable.Include(x => x.LearningTasks.OrderBy(x => x.Sort)).FirstOrDefaultAsync(x => x.CreatorId == creatorId&&x.SubjectId==subjectId&&x.ProductId==productId);
    }

    public Task<LearningPlan> GetIncludeByIdAsync(Guid id)
    {
        return Queryable.Include(x => x.LearningTasks.OrderBy(x => x.Sort)).FirstOrDefaultAsync(x => x.Id == id );
    }

    public Task<List<LearningPlan>> GetWhere(Expression<Func<LearningPlan, bool>> expression)
    {
        return dbContext.LearningPlans.Where(expression).ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTableNames()
    {
        var _serviceProvider = EngineContext.Current.Resolve<IServiceProvider>();
        //先获取合成所有的进度表并拿到结果
        await using var scope = _serviceProvider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<LearningPlanDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(LearningPlan));
        return tables;
    }
}
