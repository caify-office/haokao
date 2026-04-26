using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;

public interface IVideoStorageWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取所有的视频存储处理器
    /// </summary>
    /// <returns></returns>
    List<VideoStorageHandlerViewModel> GetHandlers();

    /// <summary>
    ///  读取权限
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    Task<VideoModel> GetVideoAuth(string videoId);
}