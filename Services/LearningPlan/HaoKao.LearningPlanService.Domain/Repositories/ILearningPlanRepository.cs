
namespace HaoKao.LearningPlanService.Domain.Repositories;

public interface ILearningPlanRepository : IRepository<LearningPlan>
{
    Task<LearningPlan> GetIncludeByCreatorIdAsync(Guid creatorId, Guid subjectId, Guid productId);

    Task<LearningPlan> GetIncludeByIdAsync(Guid id);
    Task<IReadOnlyList<string>> GetTableNames();

    Task<List<LearningPlan>> GetWhere(Expression<Func<LearningPlan, bool>> expression);

}
