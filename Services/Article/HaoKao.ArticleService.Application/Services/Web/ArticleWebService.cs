using Girvs.AuthorizePermission;
using HaoKao.ArticleService.Application.Services.Management;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace HaoKao.ArticleService.Application.Services.Web;

/// <summary>
/// 文章服务-Web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class ArticleWebService(
    IArticleService ArticleService
) : IArticleWebService
{
    #region 初始参数

    private readonly IArticleService _ArticleService = ArticleService ?? throw new ArgumentNullException(nameof(ArticleService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseArticleViewModel> Get(Guid id)
    {
        return _ArticleService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<ArticleQueryViewModel> Get([FromQuery] ArticleQueryViewModel queryViewModel)
    {
        queryViewModel.IsDisplayedOnHomepage = true;
        queryViewModel.IsPublish = true;
        return _ArticleService.Get(queryViewModel);
    }

    /// <summary>
    /// 获取当前栏目具体类别是否有更新
    /// </summary>
    /// <param name="ColumnId">栏目Id</param>
    /// <param name="CategoryIds">类别Ids</param>
    [HttpPost("{ColumnId}")]
    public async Task<Dictionary<Guid, bool>> Get(Guid ColumnId, [FromBody] Guid[] CategoryIds)
    {
        var queryViewModel = new ArticleQueryViewModel();
        queryViewModel.Column = ColumnId;
        queryViewModel.PageIndex = 1;
        queryViewModel.PageSize = 1000;
        queryViewModel.IsDisplayedOnHomepage = true;
        queryViewModel.IsPublish = true;
        var query = await _ArticleService.Get(queryViewModel);
        var result = new Dictionary<Guid, bool>();
        var now = DateTime.Now;
        CategoryIds.ToList().ForEach(categoryId =>
        {
            var isUpdated = query.Result.Any(x => x.Category == categoryId && (now - x.UpdateTime).Days < 3);
            result.TryAdd(categoryId, isUpdated);
        });
        return result;
    }

    /// <summary>
    /// 获取本周热门文章
    /// </summary>
    [HttpGet]
    public Task<List<ThisWeekHotArticleViewModel>> GetThisWeekHotArticle()
    {
        return _ArticleService.GetThisWeekHotArticle();
    }

    #endregion
}