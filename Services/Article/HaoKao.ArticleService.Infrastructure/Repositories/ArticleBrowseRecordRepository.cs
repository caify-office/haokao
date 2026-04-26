using HaoKao.ArticleService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.ArticleService.Infrastructure.Repositories;

public class ArticleBrowseRecordRepository : Repository<ArticleBrowseRecord>, IArticleBrowseRecordRepository
{
    public async Task<Dictionary<Guid, int>> GetViewCount(Guid[] articleIds)
    {
        var ids = articleIds.Distinct();
        var reusltDb = from ab in Queryable.Where(x=>ids.Contains(x.ArticleId))
                     group ab by ab.ArticleId into g
                     select new
                     {
                         ArticleId = g.Key,
                         ViewCount = g.Count(),
                     };

        var result = await reusltDb.ToListAsync();
        return result.ToDictionary(x => x.ArticleId, x => x.ViewCount);
    }
}
