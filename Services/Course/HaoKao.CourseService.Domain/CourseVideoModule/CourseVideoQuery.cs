namespace HaoKao.CourseService.Domain.CourseVideoModule;

public class CourseVideoQuery : QueryBase<CourseVideo>
{
    /// <summary>
    /// 关联的知识点id
    /// </summary>
    [QueryCacheKey]
    public Guid? KnowledgePointId { get; set; }
    /// <summary>
    /// 课程章节id
    /// </summary>
    [QueryCacheKey]
    public Guid? CourseChapterId { get; set; }

    [QueryCacheKey]
    public List<Guid> CourseChapterIds { get; set; }

    public override Expression<Func<CourseVideo, bool>> GetQueryWhere()
    {
        Expression<Func<CourseVideo, bool>> expression = x => true;

        if (KnowledgePointId.HasValue)
        {
            expression = expression.And(x => x.KnowledgePointId == KnowledgePointId);
        }
        if (CourseChapterId.HasValue)
        {
            expression = expression.And(x => x.CourseChapterId == CourseChapterId);
        }
        else
        {
            if (CourseChapterIds != null && CourseChapterIds.Count > 0)
            {
                expression = expression.And(x => CourseChapterIds.Contains(x.CourseChapterId));
            }
            else
            {
                expression = expression.And(x => x.CourseChapterId == Guid.Empty);
            }
        }
        return expression;
    }
}