using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;
using HaoKao.OpenPlatformService.Domain.Commands.AccessClient;
using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Queries;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

/// <summary>
/// 客户端管理接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "客户端管理",
    "97f09ea5-0806-4334-a65b-6dad4771dcb9",
    "32",
    SystemModule.SystemModule,
    3
)]
public class ClientService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IAccessClientRepository repository
) : IClientService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IAccessClientRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 根据查询条件获取对应的客户端
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<ClientQueryListViewModel> Get([FromQuery] ClientQueryListViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<AccessClientQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<AccessClient>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<ClientQueryListViewModel, AccessClient>();
    }

    /// <summary>
    /// 根据主键获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.AdminUser | UserType.SpecialUser)]
    public async Task<BrowseClientViewModel> Get(Guid id)
    {
        var accessClient = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(id.ToString()),
            async () => await _repository.GetById(id)
        );

        if (accessClient == null) throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return accessClient.ConvertToViewModel();
    }

    /// <summary>
    /// 根据ClientId获取详情
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("{clientId}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.AdminUser | UserType.SpecialUser)]
    public async Task<BrowseClientViewModel> GetByClientId(string clientId)
    {
        var accessClient = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<AccessClient>.QueryCacheKey.Create($"clientId={clientId}"),
            async () => await _repository.GetByClientId(clientId)
        );

        if (accessClient == null) throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return accessClient.ConvertToViewModel();
    }

    /// <summary>
    /// 创建客户端
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post,
                                       UserType.AdminUser | UserType.SpecialUser)]
    public async Task Create([FromBody] CreateClientViewModel model)
    {
        var command = model.ConvertToCommand();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 更新客户端
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit,
                                       UserType.AdminUser | UserType.SpecialUser)]
    public async Task Update([FromBody] UpdateClientViewModel model)
    {
        var command = model.ConvertToCommand();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 删除客户端
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete,
                                       UserType.AdminUser | UserType.SpecialUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteAccessClientCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}