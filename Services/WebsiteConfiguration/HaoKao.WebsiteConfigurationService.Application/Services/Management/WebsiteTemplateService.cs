using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.BusinessBasis.QueryTypeFields;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;
using HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Management;

/// <summary>
/// 模板接口服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "模板管理",
    "e857fb60-db3f-55fa-5afb-df2350449af3",
    "32",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class WebsiteTemplateService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IWebsiteTemplateRepository repository
) : IWebsiteTemplateService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IWebsiteTemplateRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseWebsiteTemplateViewModel> Get(Guid id)
    {
        var template = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<WebsiteTemplate>.ByIdCacheKey.Create(id.ToString()),
              () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的模板不存在", StatusCodes.Status404NotFound);

        return template.MapToDto<BrowseWebsiteTemplateViewModel>();
    }

    /// <summary>
    /// 根据域名获取符合条件得模板
    /// </summary>
    /// <param name="model">域名</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<BrowseWebsiteTemplateViewModel>> GetByDomainName([FromBody] QueryByDomainNameViewModel model)
    {
        var template = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<WebsiteTemplate>.QueryCacheKey.Create($"domainName={model.DomainName}"),
            () => _repository.GetByDomainNameAsync(model.DomainName)
        );

        return template.MapTo<List<BrowseWebsiteTemplateViewModel>>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<WebsiteTemplateQueryViewModel> Get([FromQuery] WebsiteTemplateQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<WebsiteTemplateQuery>();
        query.QueryFields = typeof(WebsiteTemplateQueryListViewModel).GetTypeQueryFields();
        var cacheKey = GirvsEntityCacheDefaults<WebsiteTemplate>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<WebsiteTemplateQueryViewModel, WebsiteTemplate>();
    }

    /// <summary>
    /// 创建模板
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<Guid> Create([FromBody] CreateWebsiteTemplateViewModel model)
    {
        model.Id = Guid.NewGuid();

        var command = model.MapToCommand<CreateWebsiteTemplateCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
        return command.Id;
    }

    /// <summary>
    /// 根据主键删除指定模板
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteWebsiteTemplateCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定模板
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPatch("{id}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateWebsiteTemplateViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateWebsiteTemplateCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置模板内容
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="content">模板内容</param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("设置模版内容", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Content(Guid id, [FromBody] SetWebsiteTemplateContentViewModel content)
    {
        var command = new SetWebsiteTemplateContentCommand(id, content.Content);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("启用", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Enable(Guid id)
    {
        var command = new SetWebsiteTemplateIsEnableCommand(id, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }


    /// <summary>
    /// 禁用
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task DisEnable(Guid id)
    {
        var command = new SetWebsiteTemplateIsEnableCommand(id, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }


    /// <summary>
    /// 设置为默认
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("设为默认", Permission.Edit_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Default(Guid id)
    {
        var command = new SetWebsiteTemplateDefaultCommand(id, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 取消默认
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("取消默认", Permission.Edit_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task CancelDefault(Guid id)
    {
        var command = new SetWebsiteTemplateDefaultCommand(id, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}