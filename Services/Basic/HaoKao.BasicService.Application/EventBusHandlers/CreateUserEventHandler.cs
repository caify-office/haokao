namespace HaoKao.BasicService.Application.EventBusHandlers;

public class CreateUserEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILogger<CreateUserEventHandler> logger,
    IServiceProvider serviceProvider
) : GirvsIntegrationEventHandler<CreateUserEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateUserEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateUserEvent))]
    public override async Task Handle(CreateUserEvent @event, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        //需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation($"Handling 'CreateUserEvent' eventId:{@event.Id}");

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync(
            @event.Id.ToString(), timeSpan, async () =>
            {
                var command = new EventCreateUserCommand(
                    @event.UserAccount,
                    @event.UserPassword.ToMd5(),
                    @event.UserName,
                    @event.ContactNumber,
                    DataState.Enable,
                    UserType.TenantAdminUser, //此处创建的为租户管理员
                    @event.TenantId,
                    @event.TenantName
                );
                await _bus.SendCommand(command, cancellationToken);
                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError(
                        $"Handling 'CreateUserEvent' event Error Code:{400},Message:{errorMessage}",
                        @event);
                }
            });
    }
}