using Girvs.Extensions;

namespace HaoKao.LearningPlanService.Domain.Queries;

public class LearningTaskQuery : QueryBase<LearningTask>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ProductId { get; set; }
    /// <summary>
    /// 对应的科目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 计划开始该任务的时间点
    /// </summary>
    [QueryCacheKey]
    public  DateOnly? ScheduledTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [QueryCacheKey]
    public Guid? CreatorId { get; set; }
    public override Expression<Func<LearningTask, bool>> GetQueryWhere()
    {
        Expression<Func<LearningTask, bool>> expression = x => true;
        if (ProductId.HasValue)
        {
            expression = expression.And(x => x.LearningPlan.ProductId == ProductId);
        }

        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.LearningPlan.SubjectId == SubjectId);
        }

        if (ScheduledTime.HasValue)
        {
            expression = expression.And(x => x.ScheduledTime == ScheduledTime);
        }

        if (CreatorId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == CreatorId);
        }
        return expression;
    }
}
