using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播产品包接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "直播产品包",
    "187f9f36-d773-49d2-a2e4-4f8a3f8a6fe3",
    "256",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class LiveProductPackageService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveProductPackageRepository repository,
    ILiveMessageService liveMessageService) : ILiveProductPackageService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILiveProductPackageRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILiveMessageService _liveMessageService = liveMessageService ?? throw new ArgumentNullException(nameof(liveMessageService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseLiveProductPackageViewModel> Get(Guid id)
    {
        var liveProductPackage = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveProductPackage>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的直播产品包不存在", StatusCodes.Status404NotFound);

        return liveProductPackage.MapToDto<BrowseLiveProductPackageViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<LiveProductPackageQueryViewModel> Get([FromQuery] LiveProductPackageQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LiveProductPackageQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveProductPackage>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<LiveProductPackageQueryViewModel, LiveProductPackage>();
    }

    /// <summary>
    /// 创建直播产品包
    /// </summary>
    /// <param name="models">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] List<CreateLiveProductPackageViewModel> models)
    {
        var createModel = models.MapTo<List<CreateLiveProductPackageModel>>();
        var command = new CreateLiveProductPackageCommand(createModel);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定直播产品包
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteLiveProductPackageCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定直播产品包
    /// </summary>
    /// <param name="models">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] List<UpdateLiveProductPackageViewModel> models)
    {
        var updateModels = models.MapTo<List<UpdateLiveProductPackageModel>>();
        var command = new UpdateLiveProductPackageCommand(
            updateModels
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("上架", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesUp(Guid[] id)
    {
        var command = new SetLiveProductPackageShelvesCommand(id, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        var liveConpon = await Get(id.FirstOrDefault());
        await _liveMessageService.ProductOnSell(liveConpon.LiveVideoId, id);
    }

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("下架", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesDown([FromBody] Guid[] id)
    {
        var command = new SetLiveProductPackageShelvesCommand(id, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        var liveConpon = await Get(id.FirstOrDefault());
        await _liveMessageService.ProductDownSell(liveConpon.LiveVideoId, id);
    }

    #endregion
}