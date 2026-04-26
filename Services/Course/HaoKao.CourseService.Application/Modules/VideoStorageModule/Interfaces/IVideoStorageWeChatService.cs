using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;

public interface IVideoStorageWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取所有的视频存储处理器
    /// </summary>
    /// <returns></returns>
    List<VideoStorageHandlerViewModel> GetHandlers();

    /// <summary>
    /// 根据videoid读取视频的url
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    Task<VideoPlayInfo> GetVideoPlayInfo(string videoId);
}