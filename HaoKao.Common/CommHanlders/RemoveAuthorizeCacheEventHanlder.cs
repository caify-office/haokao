using HaoKao.Common.Events.Authorize;

namespace HaoKao.Common.CommHanlders;

public class RemoveAuthorizeCacheEventHanlder(
    IStaticCacheManager staticCacheManager,
    ILogger<RemoveAuthorizeCacheEventHanlder> logger,
    ILocker locker)
    : IIntegrationEventHandler<RemoveAuthorizeCacheEvent>
{
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
    private readonly ILogger<RemoveAuthorizeCacheEventHanlder> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));

    [CapSubscribe(nameof(RemoveAuthorizeCacheEvent))]
    public async Task Handle(RemoveAuthorizeCacheEvent @event, [FromCap] CapHeader header,
                             CancellationToken cancellationToken)
    {
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);
        await _locker.PerformActionWithLockAsync(@event.Id.ToString(),
                                            TimeSpan.FromMinutes(30),
                                            () =>
                                            {
                                                try
                                                {
                                                    _staticCacheManager.RemoveByPrefix(GirvsAuthorizePermissionCacheKeyManager
                                                                                           .CurrentUserAuthorizeCacheKeyPrefix);
                                                }
                                                catch (Exception e)
                                                {
                                                    _logger.LogError(e.Message, e);
                                                }
                                                return Task.CompletedTask;
                                            });
    }
}