namespace HaoKao.OpenPlatformService.Domain.Extensions;

public static class StringExtensions
{
    public static CacheKey CreateDomainProxyCacheKey(this string domain)
    {
        var key = new CacheKey(nameof(DomainProxy).ToLowerInvariant() + ":TenantId_:byid:{0}");
        // 创建缓存Key
        var cacheKey = key.Create(domain);
        return cacheKey;
    }
}