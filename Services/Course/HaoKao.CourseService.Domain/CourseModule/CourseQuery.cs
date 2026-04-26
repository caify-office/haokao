namespace HaoKao.CourseService.Domain.CourseModule;

public class CourseQuery : QueryBase<Course>
{
    /// <summary>
    /// 课程名称
    /// </summary>
    [QueryCacheKey]
    public string CourseName { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [QueryCacheKey]
    public bool? State { get; set; }

    /// <summary>
    /// 课程ids
    /// </summary>
    [QueryCacheKey]
    public string CourseIds { get; set; }

    /// <summary>
    /// 课程类型
    /// </summary>
    [QueryCacheKey]
    public CourseType? CourseType { get; set; }

    public override Expression<Func<Course, bool>> GetQueryWhere()
    {
        Expression<Func<Course, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(CourseIds))
        {
            expression = expression.And(x => CourseIds.Contains(x.Id.ToString()));
        }
        if (!string.IsNullOrEmpty(CourseName))
        {
            expression = expression.And(x => x.Name.Contains(CourseName));
        }
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }
        if (State.HasValue)
        {
            expression = expression.And(x => x.State == State);
        }
        if (CourseType.HasValue)
        {
            expression = expression.And(x => x.CourseType == CourseType);
        }
        return expression;
    }
}