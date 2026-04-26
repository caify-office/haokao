using HaoKao.LiveBroadcastService.Domain.Entities;
using HaoKao.LiveBroadcastService.Domain.Enums;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveVideoQuery : QueryBase<LiveVideo>
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    [QueryCacheKey]
    public List<Guid> SubjectIds { get; set; }

    /// <summary>
    /// 直播状态
    /// </summary>
    [QueryCacheKey]
    public LiveStatus? LiveStatus { get; set; }

    /// <summary>
    /// 直播类型
    /// </summary>
    [QueryCacheKey]
    public LiveType? LiveType { get; set; }

    /// <summary>
    /// 距离当日的天数
    /// </summary>
    [QueryCacheKey]
    public int? DayCount { get; set; }

    /// <summary>
    /// 查询该时间之后的直播
    /// </summary>
    [QueryCacheKey]
    public DateTime? QueryStartTime { get; set; }
    /// <summary>
    /// 查询该时间之前的直播
    /// </summary>
    [QueryCacheKey]
    public DateTime? QueryEndTime { get; set; }

    public override Expression<Func<LiveVideo, bool>> GetQueryWhere()
    {
        Expression<Func<LiveVideo, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (SubjectIds!.Any())
        {
            Expression<Func<LiveVideo, bool>> subjectIdsQuery = x => false;
            SubjectIds.ForEach(subjectId => { subjectIdsQuery = subjectIdsQuery.Or(x => x.SubjectIdsStr.Contains(subjectId.ToString())); });
            expression = expression.And(subjectIdsQuery);
        }

        if (LiveStatus.HasValue)
        {
            expression = expression.And(x => x.LiveStatus == LiveStatus);
        }

        if (LiveType.HasValue)
        {
            expression = expression.And(x => x.LiveType == LiveType);
        }

        if (DayCount.HasValue)
        {
            var startTime = DateTime.Now.AddDays(-DayCount.Value).Date;
            expression = expression.And(x => x.StartTime.Date >= startTime);
        }
        if (QueryStartTime.HasValue)
        {
            expression = expression.And(x => x.StartTime.Date >= QueryStartTime.Value.Date);
        }

        if (QueryEndTime.HasValue)
        {
            expression = expression.And(x => x.StartTime.Date <= QueryEndTime.Value.Date);
        }
        return expression;
    }
}