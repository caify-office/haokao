using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Extensions;

public static class ProductCacheKeyExtensions
{
    public static CacheKey CreateProductIdsCacheKey(this IEnumerable<Guid> ids)
    {
        var key = string.Join("-", ids.OrderBy(x => x)).ToMd5();
        return GirvsEntityCacheDefaults<Product>.ByIdsCacheKey.Create(key);
    }

    public static CacheKey CreateProductIdsIncludeCacheKey(this IEnumerable<Guid> ids)
    {
        var key = string.Join("-Include", ids).ToMd5();
        return GirvsEntityCacheDefaults<Product>.ByIdsCacheKey.Create(key);
    }

    public static CacheKey CreateFreeProductIdsCacheKey(this IEnumerable<Guid> ids)
    {
        var key = string.Join("-FreeProduct", ids).ToMd5();
        return GirvsEntityCacheDefaults<Product>.ByIdsCacheKey.Create(key);
    }
}