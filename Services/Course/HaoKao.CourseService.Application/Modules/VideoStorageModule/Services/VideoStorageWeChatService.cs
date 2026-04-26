using HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;
using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Services;

/// <summary>
/// 存储器配置保存
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class VideoStorageWeChatService(IVideoStorageService service) : IVideoStorageWeChatService
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

    [HttpGet]
    public async Task<VideoPlayInfo> GetVideoPlayInfo(string videoId)
    {
        var handler = await service.GetVideoStorageHandler();
        var response = await handler.GetVideoPlayInfo(videoId);
        return response;
    }
}