using System.Collections.Generic;

namespace HaoKao.ArticleService.Application.Services.Management;

public interface IArticleService : IAppWebApiService, IManager
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
    /// 创建文章
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateArticleViewModel model);

    /// <summary>
    /// 根据主键删除指定文章
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定文章
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateArticleViewModel model);

    /// <summary>
    /// 置顶
    /// </summary>
    /// <param name="id">主键</param>
    Task SetTopping(Guid id);

    /// <summary>
    /// 取消置顶
    /// </summary>
    /// <param name="id">主键</param>
     Task CancelTopping(Guid id);

    /// <summary>
    /// 获取本周热门文章
    /// </summary>
    Task<List<ThisWeekHotArticleViewModel>> GetThisWeekHotArticle();
}