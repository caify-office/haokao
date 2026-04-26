using Girvs.BusinessBasis.Queries;
using Girvs.Extensions;
using Girvs.Extensions.Collections;
using HaoKao.ArticleService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.ArticleService.Infrastructure.Repositories;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public override async Task<List<Article>> GetByQueryAsync(QueryBase<Article> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderByDescending(x=>x.IsTopping)
                               .ThenByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public async Task<List<Article>> GetThisWeekHotArticle()
    {
        var expression = OtherQueryCondition;
        expression = expression.And(x => x.IsPublish == true && x.IsDisplayedOnHomepage == true);
        var dbContext = EngineContext.Current.Resolve<ArticleDbContext>();
        var articles = dbContext.Articles.Where(expression);
        var startTime = DateTime.Now.AddDays(-7);
        var articleBrowseRecords = dbContext.ArticleBrowseRecords.Where(x => x.CreateTime>startTime);
        var viewCount = from article in articles
                        from articleBrowseRecord in articleBrowseRecords.Where(abr=> article.Id == abr.ArticleId).DefaultIfEmpty()
                        where articleBrowseRecord != null
                        group article by article.Id into g
                        orderby  g.Count() descending
                        select new
                        {
                            ArticleId = g.Key,
                            Count = g.Count()
                        };

        var result = from vc in viewCount
                     from article in articles.Where(a=>vc.ArticleId==a.Id).DefaultIfEmpty()
                     select new Article
                     {
                         Id = vc.ArticleId,
                         Title = article.Title,
                         Category=article.Category,
                         IsPublish = article.IsPublish,
                         IsDisplayedOnHomepage = article.IsDisplayedOnHomepage,
                         PreviewUrl = article.PreviewUrl
                     };

        return await result.Take(4).ToListAsync();

    }
}
