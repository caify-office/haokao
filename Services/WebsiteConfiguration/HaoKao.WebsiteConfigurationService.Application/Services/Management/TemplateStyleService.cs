using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.WebsiteConfigurationService.Application.ViewModels.TemplateStyle;
using HaoKao.WebsiteConfigurationService.Domain.Commands.TemplateStyle;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Management;


/// <summary>
/// 模板样式接口服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "模板样式管理",
    "dfa5c231-7682-7b45-069c-e7d2df3b707d",
    "32",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class TemplateStyleService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ITemplateStyleRepository repository
) : ITemplateStyleService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ITemplateStyleRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<TemplateStyleQueryViewModel> Get([FromQuery] TemplateStyleQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<TemplateStyleQuery>();
        var cacheKey = GirvsEntityCacheDefaults<TemplateStyle>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<TemplateStyleQueryViewModel, TemplateStyle>();
    }

    /// <summary>
    /// 创建模板样式
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateTemplateStyleViewModel model)
    {
        var command = model.MapToCommand<CreateTemplateStyleCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定模板样式
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteTemplateStyleCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}