using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Infrastructure.Repositories;

public class ProductPackageRepository : Repository<ProductPackage>, IProductPackageRepository
{
    public async Task<List<Guid>> GetAllProductIds()
    {
        var productIds = await Queryable.Select(x => x.ProductList).ToListAsync();
        return productIds.SelectMany(x => x).ToList();
    }

    public override async Task<List<ProductPackage>> GetByQueryAsync(QueryBase<ProductPackage> query)
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
                               .OrderByDescending(x => x.Hot)
                               .ThenBy(x => x.Sort)
                               .ThenByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize)
                               .ToListAsync();
        }

        return query.Result;
    }
}