using Girvs.AuthorizePermission;
using Girvs.Extensions;
using HaoKao.ArticleService.Application.ViewModels.ArticleBrowseRecord;
using HaoKao.ArticleService.Domain.Commands.ArticleBrowseRecord;
using HaoKao.ArticleService.Domain.Entities;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace HaoKao.ArticleService.Application.Services.Web;

/// <summary>
/// 文章浏览记录接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class ArticleBrowseRecordWebService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IArticleBrowseRecordRepository repository
) : IArticleBrowseRecordWebService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IArticleBrowseRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法





    /// <summary>
    /// 创建文章浏览记录
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody]CreateArticleBrowseRecordViewModel model)
    {
        var command = new CreateArticleBrowseRecordCommand(
            model.ClientUniqueId
            ,model.ArticleId
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取文章浏览量
    /// </summary>
    /// <param name="articleIds">文章Ids</param>
    [HttpPost]
    public async Task<Dictionary<Guid,int>> GetViewCount([FromBody] Guid[] articleIds)
    {
        var cacheKeyStr = string.Join("_", articleIds.OrderBy(x => x)).ToMd5();
        var result = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ArticleBrowseRecord>.QueryCacheKey.Create(cacheKeyStr), async () =>
            {
                return await _repository.GetViewCount(articleIds);
            });

        //补充没有浏览记录的文章浏览数
        articleIds.ToList().ForEach(x =>
        {
            if (!result.ContainsKey(x))
            {
                result.Add(x, 0);
            }
        });
        return result;
    }
    #endregion
}