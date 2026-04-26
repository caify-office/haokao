using HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;
using HaoKao.CourseService.Application.Modules.VideoStorageModule.ViewModels;
using HaoKao.CourseService.Application.Storages;
using HaoKao.CourseService.Domain.VideoStorageModule;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Services;

/// <summary>
/// 存储器配置保存
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "存储设置",
    "0af04928-cc92-46f8-98d9-e4e6807b9cab",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class VideoStorageService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IVideoStorageRepository repository
) : IVideoStorageService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IVideoStorageRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseVideoStorageViewModel> Get()
    {
        var videoStorage = await GetCurrentVideoStorage();
        if (videoStorage == null) return new BrowseVideoStorageViewModel();

        var result = videoStorage.MapToDto<BrowseVideoStorageViewModel>();
        result.ConfigParameter = JsonSerializer.Deserialize<dynamic>(videoStorage.ConfigParameter);
        return result;
    }

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, UserType.TenantAdminUser)]
    public async Task Update([FromBody] SaveVideoStorageViewModel model)
    {
        var command = new SaveVideoStorageCommand(
            model.VideoStorageHandlerId,
            model.VideoStorageHandlerName,
            JsonSerializer.Serialize(model.ConfigParameter)
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取所有的视频存储处理器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<VideoStorageHandlerViewModel>> GetHandlers()
    {
        var handlers = GetVideoStorageHandlers();
        var result = new List<VideoStorageHandlerViewModel>();

        foreach (var handler in handlers)
        {
            var handlerType = handler.GetType();
            var property = handlerType.GetProperty("Config");
            if (property == null) continue;

            var config = property.GetValue(handler);

            var handlerModel = new VideoStorageHandlerViewModel
            {
                StorageHandlerId = handler.HandlerId,
                StorageHandlerName = handler.HandlerName,
                StorageConfig = config,
            };
            result.Add(handlerModel);
        }
        return Task.FromResult(result);
    }

    /// <summary>
    /// 根据当前的配置搜索视频列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> SearchVideo()
    {
        var handler = await GetVideoStorageHandler();
        var response = await handler.SearchStorage(EngineContext.Current.HttpContext.Request);
        return response;
    }

    /// <summary>
    /// 获取视频信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<VideoInfoModel> GetVideoInfo(string videoId)
    {
        try
        {
            var handler = await GetVideoStorageHandler();
            var response = await handler.GetVideoInfo(videoId);
            return response;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 根据当前视频id读取阿里视频加密视频播放密钥
    /// </summary>
    /// <param name="videoId">视频id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<VideoModel> GetVideoAuth(string videoId)
    {
        var handler = await GetVideoStorageHandler();
        var response = await handler.GetVideoAuth(videoId);
        return response;
    }

    /// <summary>
    /// 根据当前的配置获取分类列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> GetCategories()
    {
        var handler = await GetVideoStorageHandler();
        var response = await handler.GetCategories();
        return response;
    }

    #endregion

    #region NonAction

    [NonAction]
    public async Task<VideoStorage> GetCurrentVideoStorage()
    {
        var tenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>();
        var videoStorage = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<VideoStorage>.ByIdCacheKey.Create(tenantId.ToString()),
            () => _repository.GetAsync(x => x.TenantId == tenantId)
        );

        return videoStorage;
    }

    [NonAction]
    public List<IStorageHandler> GetVideoStorageHandlers()
    {
        var typeFinder = new WebAppTypeFinder();
        var handlerTypes = typeFinder.FindOfType<IStorageHandler>();
        return handlerTypes.Select(x => EngineContext.Current.Resolve(x) as IStorageHandler).ToList();
    }

    [NonAction]
    public async Task<IStorageHandler> GetVideoStorageHandler()
    {
        var videoStorage = await GetCurrentVideoStorage() ?? throw new GirvsException("对应的设置不存在", StatusCodes.Status404NotFound);
        var handlers = GetVideoStorageHandlers();
        var videoStorageHandler = handlers.FirstOrDefault(x => x.HandlerId == videoStorage.VideoStorageHandlerId) ?? throw new GirvsException("未找到对应的视频存储处理器");
        videoStorageHandler.SetConfig(videoStorage.ConfigParameter);
        return videoStorageHandler;
    }

    #endregion
}