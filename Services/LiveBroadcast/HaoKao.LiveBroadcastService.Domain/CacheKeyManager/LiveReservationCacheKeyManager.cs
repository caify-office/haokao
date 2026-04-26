using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CacheKeyManager;

public static class LiveReservationCacheKeyManager
{
    public static CacheKey CreateMyLiveReservationCacheKey(Guid userId)
    {
        var cacheKey = GirvsEntityCacheDefaults<LiveReservation>.ByIdCacheKey.Create($"MyLiveReservation:{userId}");
        return cacheKey;
    }
}