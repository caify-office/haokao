using Girvs.Driven.Bus;
using Girvs.Driven.CacheDriven.Events;

namespace HaoKao.Common.Extensions;

public static class MediatorHandlerExtension
{
    public static Task RemoveIdCacheEvent<T>(this IMediatorHandler bus, string key, CancellationToken cancellationToken) where T : Entity
    {
        return bus.RaiseEvent(new RemoveCacheEvent(GirvsEntityCacheDefaults<T>.ByIdCacheKey.Create(key)), cancellationToken);
    }

    public static Task RemoveListCacheEvent<T>(this IMediatorHandler bus, CancellationToken cancellationToken = default) where T : Entity
    {
        return bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<T>.ListCacheKey.Create()), cancellationToken);
    }

    public static Task RemoveQueryCacheEvent<T>(this IMediatorHandler bus, CancellationToken cancellationToken) where T : Entity
    {
        return bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<T>.QueryCacheKey.Create()), cancellationToken);
    }

    public static Task RemoveTenantCacheEvent<T>(this IMediatorHandler bus, CancellationToken cancellationToken) where T : Entity
    {
        return bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<T>.ByTenantKey.Create()), cancellationToken);
    }

    public static Task RemoveTenantListCacheEvent<T>(this IMediatorHandler bus, CancellationToken cancellationToken) where T : Entity
    {
        return bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<T>.TenantListCacheKey.Create()), cancellationToken);
    }

    public static Task RemoveAllCacheEvent<T>(this IMediatorHandler bus, CancellationToken cancellationToken) where T : Entity
    {
        return bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<T>.AllCacheKey.Create()), cancellationToken);
    }

    public static Task UpdateIdCacheEvent<T>(this IMediatorHandler bus, T data, string key, CancellationToken cancellationToken) where T : Entity
    {
        return bus.RaiseEvent(new SetCacheEvent(data, GirvsEntityCacheDefaults<T>.ByIdCacheKey.Create(key)), cancellationToken);
    }
}