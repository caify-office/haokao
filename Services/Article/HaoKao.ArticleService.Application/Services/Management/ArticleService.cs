

using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.ArticleService.Domain.Entities;
using HaoKao.ArticleService.Domain.Extensions;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace HaoKao.ArticleService.Application.Services.Management;

/// <summary>
/// 文章接口服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "文章管理",
    "f09ff8cb-2e2c-4cd1-8ca0-5824d071bcd7",
    "32",
       SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class ArticleService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IArticleRepository repository
) : IArticleService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IArticleRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseArticleViewModel> Get(Guid id)
    {
        var Article = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<Article>.ByIdCacheKey.Create(id.ToString()),
              async () => await _repository.GetByIdAsync(id)
            );

        if (Article == null)
            throw new GirvsException("对应的文章不存在", StatusCodes.Status404NotFound);

        return Article.MapToDto<BrowseArticleViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<ArticleQueryViewModel> Get([FromQuery] ArticleQueryViewModel queryViewModel)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
        var query = queryViewModel.MapToQuery<ArticleQuery>();
        query.TenantId = tenantId;
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Article>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<ArticleQueryViewModel, Article>();
    }

    /// <summary>
    /// 获取本周热门文章
    /// </summary>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<ThisWeekHotArticleViewModel>> GetThisWeekHotArticle()
    {
        var effectTime = (int)TimeSpan.FromDays(1).TotalMinutes;
        var reuslt = await _cacheManager.GetAsync(ArticleCacheManager.ThisWeekHotArticleCacheKey.Create(cacheTime: effectTime), async () =>
        {
            return await _repository.GetThisWeekHotArticle();
        });
        return reuslt.MapTo<List<ThisWeekHotArticleViewModel>>();
    }

    /// <summary>
    /// 创建文章
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateArticleViewModel model)
    {
        var command = model.MapToCommand<CreateArticleCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定文章
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteArticleCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定文章
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateArticleViewModel model)
    {
        var command = model.MapToCommand<UpdateArticleCommand>();
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 置顶
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch("{id}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task SetTopping(Guid id)
    {
        var command = new SetArticleIsToppingCommand(
            id,
           true
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 取消置顶
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch("{id}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task CancelTopping(Guid id)
    {
        var command = new SetArticleIsToppingCommand(
            id,
           false
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}