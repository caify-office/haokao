using HaoKao.ArticleService.Domain.Entities;
using System.Collections.Generic;

namespace HaoKao.ArticleService.Domain.Repositories;

public interface IArticleBrowseRecordRepository : IRepository<ArticleBrowseRecord>
{
    /// <summary>
    /// 获取文章浏览量
    /// </summary>
    /// <param name="articleIds">文章Ids</param>
    Task<Dictionary<Guid, int>> GetViewCount(Guid[] articleIds);
}
