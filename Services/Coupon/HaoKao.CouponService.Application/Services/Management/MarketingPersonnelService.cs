using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.CouponService.Application.ViewModels.MarketingPersonnel;
using HaoKao.CouponService.Domain.Commands.MarketingPersonnel;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Queries;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.CouponService.Application.Services.Management;

/// <summary>
/// 营销人员管理
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "营销人员管理",
    "08db9d6c-7416-49a3-81a2-59d3ce4aecbd",
    "1024",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class MarketingPersonnelService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IMarketingPersonnelRepository repository
) : IMarketingPersonnelService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IMarketingPersonnelRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseMarketingPersonnelViewModel> Get(Guid id)
    {
        var marketingPersonnel = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<MarketingPersonnel>.ByIdCacheKey.Create(id.ToString()),
              () => _repository.GetByIdAsync(id)
            ) ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return marketingPersonnel.MapToDto<BrowseMarketingPersonnelViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<MarketingPersonnelQueryViewModel> Get([FromQuery]MarketingPersonnelQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<MarketingPersonnelQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<MarketingPersonnel>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<MarketingPersonnelQueryViewModel, MarketingPersonnel>();
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody]CreateMarketingPersonnelViewModel model)
    {
        var command =model.MapToCommand<CreateMarketingPersonnelCommand>();

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
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteMarketingPersonnelCommand(id);

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
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id,[FromBody] UpdateMarketingPersonnelViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateMarketingPersonnelCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}