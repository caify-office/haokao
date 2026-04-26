using Girvs.Infrastructure;
using HaoKao.ArticleService.Domain.Entities;

namespace HaoKao.ArticleService.Domain.Extensions;

public static class ArticleCacheManager
{
    /// <summary>
    /// 获取缓存键中使用的实体类型名称
    /// </summary>
    private static string EntityTypeName => nameof(Article).ToLowerInvariant();
    private static string TenantKey
    {
        get
        {
            if (!(typeof(Article).GetProperty("TenantId") == null))
            {
                return ":TenantId_" + EngineContext.Current.ClaimManager.GetTenantId();
            }

            return string.Empty;
        }
    }

    public static CacheKey ThisWeekHotArticleCacheKey => new CacheKey($"{EntityTypeName}{TenantKey}:thisweekhotarticle:{DateTime.Now.ToString("yyyy-MM-dd")}");
}