using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CacheKeyManager;

public static class LiveAdministratorCacheKeyManager
{
    public static CacheKey CreateAdminCacheKey(Guid userId)
    {
        var cacheKey = GirvsEntityCacheDefaults<LiveAdministrator>.ByIdCacheKey.Create($"userIdIsLiveAdmin:{userId}");
        return cacheKey;
    }
}