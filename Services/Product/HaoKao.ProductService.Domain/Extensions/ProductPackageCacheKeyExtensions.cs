using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Extensions;

public static class ProductPackageCacheKeyExtensions
{
    public static CacheKey CreateProductPackageIdsCacheKey(this IEnumerable<Guid> ids)
    {
        var key = string.Join("-", ids.OrderBy(x => x)).ToMd5();
        return GirvsEntityCacheDefaults<ProductPackage>.ByIdsCacheKey.Create(key);
    }
}