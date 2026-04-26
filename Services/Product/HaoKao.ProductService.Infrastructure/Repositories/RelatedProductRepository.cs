using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Infrastructure.Repositories;

public class RelatedProductRepository : Repository<RelatedProduct>, IRelatedProductRepository
{
    public override async Task<List<RelatedProduct>> GetByQueryAsync(QueryBase<RelatedProduct> query)
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
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public Task<List<Product>> GetRelatedProductByIds(Guid[] ids)
    {
        return Queryable.Include(x => x.Product)
                        .Where(x => ids.Contains(x.RelatedTargetProductId))
                        .Select(x => x.Product)
                        .GroupBy(x => new { x.Id })
                        .Select(g => g.First())
                        .ToListAsync();
    }
}