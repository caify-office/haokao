using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveVideo;

[AutoMapFrom(typeof(LiveVideoQuery))]
[AutoMapTo(typeof(LiveVideoQuery))]
public class LiveVideoQueryViewModel : QueryDtoBase<LiveVideoQueryListViewModel>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public List<Guid> SubjectIds { get; set; }

    /// <summary>
    /// 直播状态
    /// </summary>
    public LiveStatus? LiveStatus { get; set; }

    /// <summary>
    /// 直播类型
    /// </summary>
    public LiveType? LiveType { get; set; }

    /// <summary>
    /// 距离当日的天数
    /// </summary>
    public int? DayCount { get; set; }
    /// <summary>
    /// 查询该时间之后的直播
    /// </summary>
    public DateTime? QueryStartTime { get; set; }
    /// <summary>
    /// 查询该时间之前的直播
    /// </summary>
    public DateTime? QueryEndTime { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.LiveVideo))]
[AutoMapTo(typeof(Domain.Entities.LiveVideo))]
public class LiveVideoQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 卡片Url
    /// </summary>

    public string CardUrl { get; set; }

    /// <summary>
    /// 直播类型
    /// </summary>
    public LiveType LiveType { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public List<Guid> SubjectIds { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public List<string> SubjectNames { get; set; }

    /// <summary>
    /// 主讲老师Id
    /// </summary>
    public List<Guid> LecturerId { get; set; }

    /// <summary>
    /// 主讲老师名称
    /// </summary>
    public List<string> LecturerName { get; set; }

    /// <summary>
    /// 直播开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 直播结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 直播状态
    /// </summary>
    public LiveStatus LiveStatus { get; set; }

    /// <summary>
    /// 购买产品跳转对象Id
    /// </summary>
    public Guid TargetProductId { get; set; }

    /// <summary>
    /// 详情介绍
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 播流地址
    /// </summary>
    public Dictionary<string, string> LiveAddress { get; set; }

    /// <summary>
    /// 推流地址
    /// </summary>
    public string StreamingAddress { get; set; }

    /// <summary>
    /// 直播公告Id
    /// </summary>
    public Guid? LiveAnnouncementId { get; set; }

    /// <summary>
    /// 是否上次回放视频
    /// </summary>
    public bool IsUploadPlayBack { get; set; }
    /// <summary>
    /// 观看人数
    /// </summary>
    public int ViewNumber { get; set; }
}