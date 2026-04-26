using DotNetCore.CAP;
using Girvs.Cache.Configuration;
using Girvs.EventBus;
using Girvs.EventBus.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common.Events.Coupon;
using HaoKao.CouponService.Domain.Commands.UserCoupon;
using HaoKao.CouponService.Domain.Enumerations;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace HaoKao.CouponService.Application.EventBusHandlers;

public class CreateUserCouponEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    ILogger<CreateUserCouponEventHandler> logger,
    INotificationHandler<DomainNotification> notifications,
    IServiceProvider serviceProvider)
    : GirvsIntegrationEventHandler<CreateUserCouponEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateUserCouponEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateUserCouponEvent))]
    public override async Task Handle(CreateUserCouponEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);
        _logger.LogInformation($"Handling 'CreateUserCouponEvent' eventId:{e.Id}");
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                var command = new CreateUserCouponEventCommand
                (
                    e.CouponId,
                    e.NickName,
                    (ChannelType)e.ChannelType
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError($"Handling 'CreateUserCouponEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}