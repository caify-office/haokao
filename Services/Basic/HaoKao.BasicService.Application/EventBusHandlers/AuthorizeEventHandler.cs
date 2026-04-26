namespace HaoKao.BasicService.Application.EventBusHandlers;

public class AuthorizeEventHandler(
    ILocker locker,
    ILogger<AuthorizeEventHandler> logger,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IServiceProvider serviceProvider
) : GirvsIntegrationEventHandler<AuthorizeEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly ILogger<AuthorizeEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(AuthorizeEvent))]
    public override async Task Handle(AuthorizeEvent @event, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling 'AuthorizeEvent' eventId:{@event.Id}");
        //需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();

        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        var isLock = await _locker.PerformActionWithLockAsync(
            @event.Id.ToString(), timeSpan, async () =>
            {
                var servicePermissionCommandModels = new List<ServicePermissionCommandModel>();
                foreach (var permissionAuthorize in @event.AuthorizePermissions)
                {
                    servicePermissionCommandModels.Add(new ServicePermissionCommandModel
                    {
                        ServiceName = permissionAuthorize.ServiceName,
                        ServiceId = permissionAuthorize.ServiceId,
                        Permissions = permissionAuthorize.Permissions,
                        OperationPermissionModels = permissionAuthorize.OperationPermissionModels,
                        Order = permissionAuthorize.Order,
                        Tag = permissionAuthorize.Tag,
                        FuncModule = permissionAuthorize.SystemModule,
                        OtherParams = permissionAuthorize.OtherParams
                    });
                }

                var serviceDataRuleCommandModels = new List<ServiceDataRuleCommandModel>();

                foreach (var authorizeDataRule in @event.AuthorizeDataRules)
                {
                    foreach (var dataRuleFieldModel in authorizeDataRule.AuthorizeDataRuleFieldModels)
                    {
                        serviceDataRuleCommandModels.Add(new ServiceDataRuleCommandModel
                        {
                            EntityTypeName = authorizeDataRule.EntityTypeName,
                            EntityDesc = authorizeDataRule.EntityDesc,
                            FieldName = dataRuleFieldModel.FieldName,
                            FieldType = dataRuleFieldModel.FieldType,
                            FieldValue = dataRuleFieldModel.FieldValue,
                            ExpressionType = dataRuleFieldModel.ExpressionType,
                            FieldDesc = dataRuleFieldModel.FieldDesc,
                            UserType = dataRuleFieldModel.UserType,
                            Order = authorizeDataRule.Order,
                            Tag = authorizeDataRule.Tag
                        });
                    }
                }

                var command = new NeedAuthorizeListCommand(servicePermissionCommandModels, serviceDataRuleCommandModels);

                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError(
                        $"Handling 'AuthorizeEvent' event Error Code:{400},Message:{errorMessage}",
                        @event);
                }
            });

        _logger.LogInformation($"本次事件处理过程中，Redis锁的情况为：{isLock} 事件ID为：{@event.Id}");
    }
}