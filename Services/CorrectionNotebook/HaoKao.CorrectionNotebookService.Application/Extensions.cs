using Girvs.Driven.CacheDriven.Events;

namespace HaoKao.CorrectionNotebookService.Application;

public static class IMediatorHandlerExtensions
{
    public static async Task<T> TrySendCommand<T>(
        this IMediatorHandler bus, IRequest<T> request,
        DomainNotificationHandler notifications,
        CancellationToken cancellationToken = default
    )
    {
        var result = await bus.SendCommand(request, cancellationToken);

        if (notifications.HasNotifications())
        {
            var errorMessage = notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return (T)result;
    }

    public static Task RemoveCache(this IMediatorHandler bus, CacheKey cacheKey, CancellationToken cancellationToken = default)
    {
        return bus.RaiseEvent(new RemoveCacheListEvent(cacheKey.Create()), cancellationToken);
    }
}

public class ServiceCacheKey<T> where T : class
{
    private static Guid UserId =>
        EngineContext.Current.IsAuthenticated
            ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>()
            : Guid.Empty;

    private static string Prefix => $"correction_notebook:user_{UserId}:{typeof(T).Name.ToLower()}";

    public static CacheKey ListCacheKey => new CacheKey($"{Prefix}:list").Create();

    public static CacheKey QueryCacheKey => new($"{Prefix}:list:{{0}}");

    public static CacheKey IdCacheKeyPrefix => new($"{Prefix}:id");

    public static CacheKey IdCacheKey => new($"{Prefix}:id:{{0}}");
}