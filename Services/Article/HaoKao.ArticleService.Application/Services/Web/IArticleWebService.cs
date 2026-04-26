using System.Collections.Generic;

namespace HaoKao.ArticleService.Application.Services.Web;

public interface IArticleWebService : IAppWebApiService
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseArticleViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<ArticleQueryViewModel> Get(ArticleQueryViewModel queryViewModel);

    /// <summary>
    /// 获取当前栏目具体类别是否有更新
    /// </summary>
    /// <param name="ColumnId">栏目Id</param>
    /// <param name="CategoryIds">类别Ids</param>
    Task<Dictionary<Guid, bool>> Get(Guid ColumnId, [FromBody] Guid[] CategoryIds);

    /// <summary>
    /// 获取本周热门文章
    /// </summary>
    Task<List<ThisWeekHotArticleViewModel>> GetThisWeekHotArticle();

}