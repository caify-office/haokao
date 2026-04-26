using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using HaoKao.Common;
using HaoKao.GroupBookingService.Application.ViewModels.GroupData;
using HaoKao.GroupBookingService.Domain.Commands.GroupData;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Queries.EntityQuery;
using HaoKao.GroupBookingService.Domain.Repositories;

namespace HaoKao.GroupBookingService.Application.Services.Management;

/// <summary>
/// 管理端拼团资料接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "拼团资料管理",
    "9b93563a-ccc6-8fc4-92b8-73493397177c",
    "512",
    SystemModule.ExtendModule2,
    1
)]
public class GroupDataService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IGroupDataRepository repository
) : IGroupDataService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IGroupDataRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 查询服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseGroupDataViewModel> Get(Guid id)
    {
        var groupData = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<GroupData>.ByIdCacheKey.Create(id.ToString()),
            async () => await _repository.GetByIdAsync(id)
        );

        if (groupData == null)
        {
            throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);
        }

        return groupData.MapToDto<BrowseGroupDataViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<GroupDataQueryViewModel> Get([FromQuery] GroupDataQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<GroupDataQuery>();
        var tempQuery = await _cacheManager.GetAsync(GirvsEntityCacheDefaults<GroupData>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });

        if (!tempQuery.Equals(query))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<GroupDataQueryViewModel, GroupData>();
    }

    #endregion

    #region 增加，删除，修改 服务方法

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateGroupDataViewModel model)
    {
        var command = model.MapToCommand<CreateGroupDataCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteGroupDataCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateGroupDataViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateGroupDataCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}