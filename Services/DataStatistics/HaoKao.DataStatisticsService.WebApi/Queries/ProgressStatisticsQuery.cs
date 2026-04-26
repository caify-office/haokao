using HaoKao.DataStatisticsService.WebApi.Enums;
using HaoKao.DataStatisticsService.WebApi.Models;

namespace HaoKao.DataStatisticsService.WebApi.Queries;

public class ProgressStatisticsQuery
{
    /// <summary>
    /// 手机号/用户名称
    /// </summary>
    public string PhoneOrNickName { get; set; }

    /// <summary>
    /// 上次学习开始时间最小值
    /// </summary>
    public DateTime? MinLearnTime { get; set; }

    /// <summary>
    /// 上次学习结束时间最大值
    /// </summary>
    public DateTime? MaxLearnTime { get; set; }

    /// <summary>
    /// 距上次时隔天数最小值
    /// </summary>
    public int? MinLearnIntervalDays { get; set; }

    /// <summary>
    /// 距上次时隔天数最大值
    /// </summary>
    public int? MaxLearnIntervalDays { get; set; }

    /// <summary>
    /// 最小课程学习进度
    /// </summary>
    public float? MinCourseRatio { get; set; }

    /// <summary>
    /// 最大课程学习进度
    /// </summary>
    public float? MaxCourseRatio { get; set; }

    /// <summary>
    /// 最小答题进度
    /// </summary>
    public float? MinAnswerRatio { get; set; }

    /// <summary>
    /// 最大答题进度
    /// </summary>
    public float? MaxAnswerRatio { get; set; }

    /// <summary>
    /// 课程权限状态
    /// </summary>
    public PermissionExpiryType? PermissionExpiryType1 { get; set; }

    /// <summary>
    /// 题库权限状态
    /// </summary>
    public PermissionExpiryType? PermissionExpiryType2 { get; set; }

    /// <summary>
    /// 是否付费学员
    /// </summary>
    public bool? IsPaidStudent { get; init; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int RecordCount { get; set; }

    public List<ProgressStatistics> Result { get; set; }

    public int PageStart => (PageIndex - 1) * PageSize;

    public int PageCount => (int)Math.Ceiling(RecordCount / (decimal)PageSize);

    public Expression<Func<ProgressStatistics, bool>> GetQueryWhere()
    {
        Expression<Func<ProgressStatistics, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(PhoneOrNickName))
        {
            expression = expression.And(x =>(x.Phone!=null&& x.Phone.Contains(PhoneOrNickName))  || (x.NickName!=null&&x.NickName.Contains(PhoneOrNickName)));
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

        if (MinAnswerRatio.HasValue)
        {
            expression = expression.And(x => x.AnswerRadio >= MinAnswerRatio);
        }

        if (MaxAnswerRatio.HasValue)
        {
            expression = expression.And(x => x.AnswerRadio <= MaxAnswerRatio);
        }

        if (PermissionExpiryType1.HasValue)
        {
            expression = expression.And(x => x.PermissionExpiryType1 == PermissionExpiryType1.Value);
        }

        if (PermissionExpiryType2.HasValue)
        {
            expression = expression.And(x => x.PermissionExpiryType2 == PermissionExpiryType2.Value);
        }

        if (IsPaidStudent.HasValue)
        {
            expression = expression.And(x => x.IsPaidStudent == IsPaidStudent);
        }

        return expression;
    }
}