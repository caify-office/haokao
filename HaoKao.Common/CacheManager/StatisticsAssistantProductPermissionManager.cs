namespace HaoKao.Common.CacheManager;

public class StatisticsAssistantProductPermissionManager
{
    public static CacheKey BuildCacheKey(string tenantId, Guid productId, Guid subjectId)
    {
        return new CacheKey($"{"StatisticsAssistantProductPermissionTaskCountDurations".ToLowerInvariant()}:tenantId_{tenantId}:productId_{productId}:subjectId_{subjectId}:{{0}}");
    }
}