using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LivePlayBack;

[AutoMapFrom(typeof(LivePlayBackQuery))]
[AutoMapTo(typeof(LivePlayBackQuery))]
public class LivePlayBackQueryViewModel : QueryDtoBase<LivePlayBackQueryListViewModel>
{
    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    public Guid? LiveVideoId { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.LivePlayBack))]
[AutoMapTo(typeof(Domain.Entities.LivePlayBack))]
public class LivePlayBackQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 视频格式
    /// </summary>
    public VideoType VideoType { get; set; }

    /// <summary>
    /// 视频时长
    /// </summary>
    public decimal Duration { get; set; }

    /// <summary>
    /// 视频序号
    /// </summary>
    public string VideoNo { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}