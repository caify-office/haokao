using HaoKao.ArticleService.Domain.Entities;
using System.Collections.Generic;

namespace HaoKao.ArticleService.Domain.Repositories;

public interface IArticleRepository : IRepository<Article>
{
    /// <summary>
    /// 获取本周热门文章
    /// </summary>
    /// <returns></returns>
    Task<List<Article>> GetThisWeekHotArticle();
}
