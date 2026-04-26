using HaoKao.DataStatisticsService.WebApi.Enums;
using HaoKao.DataStatisticsService.WebApi.Models;

namespace HaoKao.DataStatisticsService.WebApi.Queries;

public class SendNotificationMessageQuery
{
    /// <summary>
    /// 接收渠道
    /// </summary>
    public EventReceivingChannel EventReceivingChannel { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public EventNotificationMessageType EventNotificationMessageType { get; set; }

    /// <summary>
    /// 距上次时隔天数最小值
    /// </summary>
    public int? MinDays { get; set; }

    /// <summary>
    /// 距上次时隔天数最大值
    /// </summary>
    public int? MaxDays { get; set; }

    /// <summary>
    /// 最小课程学习进度
    /// </summary>
    public float? MinCourseRatip { get; set; }

    /// <summary>
    /// 最大课程学习进度
    /// </summary>
    public float? MaxCourseRatip { get; set; }

    /// <summary>
    /// 权限状态
    /// </summary>
    public PermissionExpiryType? ExpiryType { get; set; }

    /// <summary>
    /// 接收者
    /// </summary>
    public List<string> Phones { get; set; } = [];

    public Expression<Func<ProgressStatistics, bool>> GetQueryWhere()
    {
        Expression<Func<ProgressStatistics, bool>> expression = x => x.TenantId == EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();

        if (MinDays.HasValue)
        {
            var time = DateTime.Now.AddDays(-MinDays.Value);
            expression = expression.And(x => x.LastLearnTime <= time);
        }

        if (MinDays.HasValue)
        {
            var time = DateTime.Now.AddDays(-MinDays.Value);
            expression = expression.And(x => x.LastLearnTime >= time);
        }

        if (MinCourseRatip.HasValue)
        {
            expression = expression.And(x => x.CourseRatio >= MinCourseRatip);
        }

        if (MaxCourseRatip.HasValue)
        {
            expression = expression.And(x => x.CourseRatio <= MaxCourseRatip);
        }

        return expression;
    }
}

