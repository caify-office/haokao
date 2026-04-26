using HaoKao.CourseRatingService.Domain.Entities;
using HaoKao.CourseRatingService.Domain.Enums;

namespace HaoKao.CourseRatingService.Domain.Queries;

public class CourseRatingQuery : QueryBase<CourseRating>
{
    /// <summary>
    /// 课程Id
    /// </summary>
    [QueryCacheKey]
    public string CourseId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [QueryCacheKey]
    public string NickName { get; set; }

    /// <summary>
    /// 课程名称
    /// </summary>
    [QueryCacheKey]
    public string CourseName { get; set; }

    /// <summary>
    /// 评价开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 评价结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 评价级别
    /// </summary>
    [QueryCacheKey]
    public int? Rating { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    [QueryCacheKey]
    public AuditState? AuditState { get; set; }

    public override Expression<Func<CourseRating, bool>> GetQueryWhere()
    {
        Expression<Func<CourseRating, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(CourseId))
        {
            expression = expression.And(x => CourseId.Contains(x.CourseId.ToString()));
        }
        if (!string.IsNullOrEmpty(NickName))
        {
            expression = expression.And(x => EF.Functions.Like(x.NickName, $"%{NickName}%"));
        }
        if (!string.IsNullOrEmpty(CourseName))
        {
            expression = expression.And(x => EF.Functions.Like(x.CourseName, $"%{CourseName}%"));
        }
        if (StartTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartTime);
        }
        if (EndTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime <= EndTime);
        }
        if (Rating.HasValue)
        {
            expression = expression.And(x => x.Rating == Rating);
        }
        if (AuditState.HasValue)
        {
            expression = expression.And(x => x.AuditState == AuditState);
        }

        return expression;
    }
}