namespace HaoKao.CourseService.Domain.CoursePracticeModule;

public class CoursePracticeQuery : QueryBase<CoursePractice>
{
    /// <summary>
    /// 关联的知识点id
    /// </summary>
    [QueryCacheKey]
    public Guid? KnowledgePointId { get; set; }

    /// <summary>
    /// 关联的课程章节Id（阶段学习为课程章节Id,智慧辅助学习为课程Id）
    /// </summary>
    [QueryCacheKey]
    public Guid? CourseChapterId { get; set; }

    public override Expression<Func<CoursePractice, bool>> GetQueryWhere()
    {
        Expression<Func<CoursePractice, bool>> expression = x => true;
        if (KnowledgePointId.HasValue)
        {
            expression = expression.And(x => x.KnowledgePointId == KnowledgePointId);
        }
        if (CourseChapterId.HasValue)
        {
            expression = expression.And(x => x.CourseChapterId == CourseChapterId);
        }
        return expression;
    }
}