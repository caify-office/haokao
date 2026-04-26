using Girvs.Cache.Caching;
using HaoKao.DataStatisticsService.WebApi.Models;

namespace HaoKao.DataStatisticsService.WebApi.CacheKeyManager;

public class ProgressStatisticsCacheKeyManager
{
    public static CacheKey All => GirvsEntityCacheDefaults<ProgressStatistics>.BuideCustomize(nameof(ProgressStatistics).ToLowerInvariant() + ":all");
}