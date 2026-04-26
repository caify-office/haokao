namespace HaoKao.StudentService.Domain;

internal static class IMediatorHandlerExtension
{
    public static Task UpdateEntityCache<T>(this IMediatorHandler source, T entity, CancellationToken cancellationToken) where T : BaseEntity<Guid>
    {
        var key = GirvsEntityCacheDefaults<T>.ByIdCacheKey.Create(entity.Id.ToString());
        return source.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
    }

    public static Task RemoveEntityCache<T>(this IMediatorHandler source, Guid id, CancellationToken cancellationToken) where T : BaseEntity<Guid>
    {
        var key = GirvsEntityCacheDefaults<T>.ByIdCacheKey.Create(id.ToString());
        return source.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
    }

    public static Task RemoveListCache<T>(this IMediatorHandler source, CancellationToken cancellationToken) where T : BaseEntity<Guid>
    {
        var listKey = GirvsEntityCacheDefaults<T>.TenantListCacheKey.Create();
        return source.RaiseEvent(new RemoveCacheListEvent(listKey), cancellationToken);
    }
}