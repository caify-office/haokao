using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LivePlayBack;

/// <summary>
/// 更新直播回放命令
/// </summary>
/// <param name="models"></param>
public record UpdateLivePlayBackCommand(
    List<UpdateLivePlayBackModel> models
) : Command("更新直播回放");

/// <summary>
/// 更新直播回放模型
/// </summary>
/// <param name="Id">主键</param>
/// <param name="LiveVideoId">所属视频直播Id</param>
/// <param name="Name">名称</param>
/// <param name="VideoType">视频格式</param>
/// <param name="Duration">视频时长</param>
/// <param name="VideoNo">视频序号</param>
/// <param name="Sort">排序</param>
public record UpdateLivePlayBackModel(
    Guid Id,
    Guid LiveVideoId,
    string Name,
    VideoType VideoType,
    decimal Duration,
    string VideoNo,
    int Sort
);