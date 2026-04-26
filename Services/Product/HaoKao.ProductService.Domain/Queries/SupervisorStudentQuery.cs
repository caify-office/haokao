using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Queries;

public class SupervisorStudentQuery : QueryBase<SupervisorStudent>
{
    /// <summary>
    /// 督学班级id
    /// </summary>
    [QueryCacheKey]
    public Guid SupervisorClassId { get; set; }

    /// <summary>
    /// 手机号/用户名称
    /// </summary>
    [QueryCacheKey]
    public string PhoneOrNickName { get; set; }

    /// <summary>
    /// 上次学习开始时间最小值
    /// </summary>
    [QueryCacheKey]
    public DateTime? MinLearnTime { get; set; }

    /// <summary>
    /// 上次学习结束时间最大值
    /// </summary>
    [QueryCacheKey]
    public DateTime? MaxLearnTime { get; set; }

    /// <summary>
    /// 距上次时隔天数最小值
    /// </summary>
    [QueryCacheKey]
    public int? MinLearnIntervalDays { get; set; }

    /// <summary>
    /// 距上次时隔天数最大值
    /// </summary>
    [QueryCacheKey]
    public int? MaxLearnIntervalDays { get; set; }

    /// <summary>
    /// 最小课程学习进度
    /// </summary>
    [QueryCacheKey]
    public float? MinCourseRatio { get; set; }

    /// <summary>
    /// 最大课程学习进度
    /// </summary>
    [QueryCacheKey]
    public float? MaxCourseRatio { get; set; }

    public override Expression<Func<SupervisorStudent, bool>> GetQueryWhere()
    {
        Expression<Func<SupervisorStudent, bool>> expression = x => x.SupervisorClassId == SupervisorClassId;

        if (!string.IsNullOrEmpty(PhoneOrNickName))
        {
            expression = expression.And(x => x.Phone.Contains(PhoneOrNickName) || x.Name.Contains(PhoneOrNickName));
        }

        if (MinLearnTime.HasValue)
        {
            expression = expression.And(x => x.LastLearnTime >= MinLearnTime);
        }

        if (MaxLearnTime.HasValue)
        {
            expression = expression.And(x => x.LastLearnTime <= MaxLearnTime);
        }

        if (MinLearnIntervalDays.HasValue)
        {
            var time = DateTime.Now.AddDays(-MinLearnIntervalDays.Value);
            expression = expression.And(x => x.LastLearnTime <= time);
        }

        if (MaxLearnIntervalDays.HasValue)
        {
            var time = DateTime.Now.AddDays(-MaxLearnIntervalDays.Value);
            expression = expression.And(x => x.LastLearnTime >= time);
        }

        if (MinCourseRatio.HasValue)
        {
            expression = expression.And(x => x.CourseRatio >= MinCourseRatio);
        }

        if (MaxCourseRatio.HasValue)
        {
            expression = expression.And(x => x.CourseRatio <= MaxCourseRatio);
        }


        return expression;
    }
}