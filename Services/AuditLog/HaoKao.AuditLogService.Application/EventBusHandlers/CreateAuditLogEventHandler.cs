using HaoKao.AuditLogService.Domain.Commands;
using HaoKao.Common.Events.AuditLogs;

namespace HaoKao.AuditLogService.Application.EventBusHandlers;

public class CreateAuditLogEventHandler(
    ILocker locker,
    ILogger<CreateAuditLogEventHandler> logger,
    INotificationHandler<DomainNotification> notifications,
    IMediatorHandler bus,
    IServiceProvider serviceProvider
) : GirvsIntegrationEventHandler<CreateAuditLogEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly ILogger<CreateAuditLogEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ??
                                                                throw new ArgumentNullException(nameof(notifications));

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    [CapSubscribe(nameof(CreateAuditLogEvent))]
    public override async Task Handle(CreateAuditLogEvent @event, [FromCap] CapHeader header,
                                      CancellationToken cancellationToken)
    {
        //需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation($"接收到事件，开始处理：{@event.Id}");
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);
        await _locker.PerformActionWithLockAsync(
            @event.Id.ToString(),
            timeSpan, async () =>
            {
                var command = new CreateAuditLogCommand(
                    @event.ServiceModuleName,
                    @event.Message,
                    @event.MessageContent,
                    @event.CreatorId,
                    @event.CreatorName,
                    @event.AddressIp,
                    (Domain.Enumerations.SourceType)@event.SourceType
                );

                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError(
                        $"Handling 'CreateUserEvent' event Error Code:500,Message:{errorMessage}",
                        @event);
                }
            }
        );
    }
}