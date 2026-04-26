using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Management;

/// <summary>
/// 栏目接口服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class ColumnService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IColumnRepository repository
) : IColumnService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IColumnRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseColumnViewModel> Get(Guid id)
    {
        var column = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Column>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的栏目不存在", StatusCodes.Status404NotFound);

        return column.MapToDto<BrowseColumnViewModel>();
    }


    /// <summary>
    /// 根据域名/父Id获取指定栏目的直接子栏目
    /// </summary>
    /// <param name="model"></param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<List<ColumnTreeModel>> Tree([FromBody] QueryColumnByDomainNameAndParentIdViewModel model)
    {
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Column>.QueryCacheKey.Create($"domainName={model.DomainName}_parentId={model.ParentId}"),
            () => _repository.GetTreeChildren(model.DomainName, model.ParentId)
        );
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<ColumnQueryViewModel> Get([FromQuery] ColumnQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<ColumnQuery>();
        var cacheKey = GirvsEntityCacheDefaults<Column>.QueryCacheKey.Create(query.GetCacheKey());
        var tempQuery = await _cacheManager.GetAsync(cacheKey, async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<ColumnQueryViewModel, Column>();
    }

    /// <summary>
    /// 创建栏目
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<Guid> Create([FromBody] CreateColumnViewModel model)
    {
        model.Id = Guid.NewGuid();
        var command = model.MapToCommand<CreateColumnCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
        return command.Id;
    }

    /// <summary>
    /// 根据主键删除指定栏目
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteColumnCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定栏目
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateColumnViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateColumnCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}