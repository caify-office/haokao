using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveAdministrator;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveAdministrator;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播管理员接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "直播管理员",
    "85a77ac3-db36-4238-89ea-9864221899e5",
    "256",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class LiveAdministratorService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveAdministratorRepository repository
) : ILiveAdministratorService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILiveAdministratorRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseLiveAdministratorViewModel> Get(Guid id)
    {
        var liveAdministrator = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveAdministrator>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的直播管理员不存在", StatusCodes.Status404NotFound);

        return liveAdministrator.MapToDto<BrowseLiveAdministratorViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<LiveAdministratorQueryViewModel> Get([FromQuery] LiveAdministratorQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LiveAdministratorQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveAdministrator>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<LiveAdministratorQueryViewModel, LiveAdministrator>();
    }

    /// <summary>
    /// 创建直播管理员
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateLiveAdministratorViewModel model)
    {
        var command = model.MapToCommand<CreateLiveAdministratorCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定直播管理员
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteLiveAdministratorCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}