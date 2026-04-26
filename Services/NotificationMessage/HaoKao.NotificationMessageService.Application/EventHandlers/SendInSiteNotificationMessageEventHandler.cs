using HaoKao.NotificationMessageService.Domain.MessageSenders;

namespace HaoKao.NotificationMessageService.Application.EventHandlers;

public class SendInSiteNotificationMessageEventHandler(
    IServiceProvider serviceProvider,
    ILocker locker,
    IStaticCacheManager staticCacheManager,
    ILogger<SendInSiteNotificationMessageEventHandler> logger,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IInSiteMessageSender inSiteMessageSender)
    :
        SendNotificationMessageAbstractEventHandler<SendInSiteNotificationMessageEvent>(serviceProvider, staticCacheManager)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
    private readonly ILogger<SendInSiteNotificationMessageEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IInSiteMessageSender _inSiteMessageSender = inSiteMessageSender ?? throw new ArgumentNullException(nameof(inSiteMessageSender));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    private async Task<InSiteMessageSetting> GetInSiteMessageSetting()
    {
        var inSiteMessageSettingRepository = EngineContext.Current.Resolve<IRepository<InSiteMessageSetting>>();
        var tenantId = Guid.Empty.ToString();
        var set = await _staticCacheManager.GetAsync(
            GirvsEntityCacheDefaults<InSiteMessageSetting>.ByIdCacheKey.Create(tenantId),
            () => inSiteMessageSettingRepository.GetAsync(x => true)
        );

        if (set == null)
        {
            throw new GirvsException(568, "未找到对应的设置");
        }

        return set;
    }

    [CapSubscribe(nameof(SendInSiteNotificationMessageEvent))]
    public override async Task Handle(
        SendInSiteNotificationMessageEvent @event,
        [FromCap] CapHeader header,
        CancellationToken cancellationToken
    )
    {
        //需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation($"Handling 'SendInSiteNotificationMessageEvent' eventId:{@event.Id}");

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync(@event.Id.ToString(), timeSpan, async () =>
        {
            var notificationMessageType = @event.EventNotificationMessageType;
            var registerUser = await GetRegisteredUser(@event.IdCard, @event.PhoneNumber);

            MessageTemplate messageTemplate = null;
            NotificationMessageSendState result;
            string message;

            try
            {
                var setting = await GetInSiteMessageSetting();
                messageTemplate = GetMessageTemplate(setting, notificationMessageType);
                _inSiteMessageSender.Parameter = @event.Parameter;
                _inSiteMessageSender.RegisteredUser = registerUser;
                _inSiteMessageSender.Setting = setting;
                (result, message) = await _inSiteMessageSender.SendAsync(messageTemplate);
            }
            catch (Exception e)
            {
                result = NotificationMessageSendState.SendFail;
                message = e.Message;
            }

            var parameterContent = ParameterContent(@event.Parameter);

            var command = new SendNotificationMessageCommand(
                parameterContent,
                messageTemplate?.Desc,
                ReceivingChannel.InSite,
                result,
                notificationMessageType,
                messageTemplate?.TemplateId,
                message,
                BuilderReceiver(registerUser, @event.PhoneNumber),
                false,
                @event.IdCard
            );

            await _bus.SendCommand(command, cancellationToken);

            if (_notifications.HasNotifications())
            {
                var errorMessage = _notifications.GetNotificationMessage();
                _logger.LogError($"Handling 'SendInSiteNotificationMessageEvent' event Error Code:500,Message:{errorMessage}", @event);
            }
        });
    }
}