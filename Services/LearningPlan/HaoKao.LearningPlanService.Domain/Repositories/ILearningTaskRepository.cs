namespace HaoKao.LearningPlanService.Domain.Repositories;

public interface ILearningTaskRepository : IRepository<LearningTask>
{
    Task<List<LearningTask>> GetWhere(Expression<Func<LearningTask, bool>> expression);
}
