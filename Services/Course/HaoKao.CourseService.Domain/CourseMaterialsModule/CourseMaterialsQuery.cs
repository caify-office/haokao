namespace HaoKao.CourseService.Domain.CourseMaterialsModule;

public class CourseMaterialsQuery : QueryBase<CourseMaterials>
{
    /// <summary>
    /// 关联的知识点id
    /// </summary>
    [QueryCacheKey]
    public Guid? KnowledgePointId { get; set; }
    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    [QueryCacheKey]
    public Guid? CourseChapterId { get; set; }

    [QueryCacheKey]
    public List<Guid> CourseChapterIds { get; set; }

    public override Expression<Func<CourseMaterials, bool>> GetQueryWhere()
    {
        Expression<Func<CourseMaterials, bool>> expression = x => true;
        if (KnowledgePointId.HasValue)
        {
            expression = expression.And(x => x.KnowledgePointId == KnowledgePointId);
        }
        if (CourseChapterId.HasValue)
        {
            expression = expression.And(x => x.CourseChapterId == CourseChapterId);
        }

        if (CourseChapterIds != null && CourseChapterIds.Count > 0)
        {
            expression = expression.And(x => CourseChapterIds.Contains(x.CourseChapterId));
        }

        return expression;
    }
}