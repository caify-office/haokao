using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Domain.Extensions;

public static class DictionariesCacheKeyExtensions
{
    /// <summary>
    /// 获取缓存键中使用的实体类型名称
    /// </summary>
    private static string EntityTypeName => nameof(Dictionaries).ToLowerInvariant();

    private static string TenantKey
    {
        get
        {
            var property = typeof(Dictionaries).GetProperty("TenantId");
            return property == null
                ? string.Empty
                : $":TenantId_{EngineContext.Current.ClaimManager.IdentityClaim?.TenantId ?? Guid.Empty.ToString()}";
        }
    }

    public static CacheKey CreateDictionariesQueryCacheKey(this string cacheKey)
    {
        return new CacheKey($"{EntityTypeName}{TenantKey}:list:query:{{0}}").Create(cacheKey);
    }
}