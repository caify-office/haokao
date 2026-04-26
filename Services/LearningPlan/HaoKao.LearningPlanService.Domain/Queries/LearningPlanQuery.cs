

namespace HaoKao.LearningPlanService.Domain.Queries;

public class LearningPlanQuery : QueryBase<LearningPlan>
{   
    public override Expression<Func<LearningPlan, bool>> GetQueryWhere()
    {
        Expression<Func<LearningPlan, bool>> expression = x => true;
        return expression;
    }
}
