using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;
using HaoKao.CourseService.Application.Storages;
using HaoKao.CourseService.Domain.VideoStorageModule;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;

public interface IVideoStorageService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    Task<BrowseVideoStorageViewModel> Get();

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(SaveVideoStorageViewModel model);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<List<VideoStorageHandlerViewModel>> GetHandlers();

    /// <summary>
    /// 获取搜索结果
    /// </summary>
    /// <returns></returns>
    Task<dynamic> SearchVideo();

    /// <summary>
    /// 获取视频信息
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    Task<VideoInfoModel> GetVideoInfo(string videoId);

    /// <summary>
    /// 获取当前租户的视频存储
    /// </summary>
    /// <returns></returns>
    Task<VideoStorage> GetCurrentVideoStorage();

    /// <summary>
    /// 获取视频存储处理器列表
    /// </summary>
    /// <returns></returns>
    List<IStorageHandler> GetVideoStorageHandlers();

    /// <summary>
    /// 获取视频存储处理器
    /// </summary>
    /// <returns></returns>
    Task<IStorageHandler> GetVideoStorageHandler();
}