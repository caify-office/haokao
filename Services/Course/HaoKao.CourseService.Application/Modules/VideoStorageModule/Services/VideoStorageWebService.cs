using HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;
using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Services;

/// <summary>
/// 存储器配置保存
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class VideoStorageWebService(IVideoStorageService service) : IVideoStorageWebService
{
    /// <summary>
    /// 获取所有的视频存储处理器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public List<VideoStorageHandlerViewModel> GetHandlers()
    {
        var result = new List<VideoStorageHandlerViewModel>();
        var handlers = service.GetVideoStorageHandlers();
        foreach (var handler in handlers)
        {
            var handlerType = handler.GetType();
            var property = handlerType.GetProperty("Config");
            if (property == null) continue;

            result.Add(new VideoStorageHandlerViewModel
            {
                StorageHandlerId = handler.HandlerId,
                StorageHandlerName = handler.HandlerName,
                StorageConfig = property.GetValue(handler),
            });
        }
        return result;
    }

    /// <summary>
    /// 根据当前视频id读取阿里视频加密视频播放密钥
    /// </summary>
    /// <param name="VideoId">视频id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<VideoModel> GetVideoAuth(string VideoId)
    {
        var handler = await service.GetVideoStorageHandler();
        var response = await handler.GetVideoAuth(VideoId);
        return response;
    }
}