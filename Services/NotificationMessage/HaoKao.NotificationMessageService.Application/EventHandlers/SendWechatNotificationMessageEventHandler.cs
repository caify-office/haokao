using HaoKao.NotificationMessageService.Domain.MessageSenders;

namespace HaoKao.NotificationMessageService.Application.EventHandlers;

public class SendWechatNotificationMessageEventHandler(
    IServiceProvider provider,
    ILocker locker,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILogger<SendWechatNotificationMessageEventHandler> logger,
    IStaticCacheManager cacheManager,
    IWechatMessageSender sender)
    :
        SendNotificationMessageAbstractEventHandler<SendWechatNotificationMessageEvent>(provider, cacheManager)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<SendWechatNotificationMessageEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IWechatMessageSender _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    #region ctor

    #endregion

    private async Task<WechatMessageSetting> GetWechatMessageSetting()
    {
        var wechatMessageSettingRepository = EngineContext.Current.Resolve<IRepository<WechatMessageSetting>>();
        var tenantId = Guid.Empty.ToString();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<WechatMessageSetting>.ByIdCacheKey.Create(tenantId),
            () => wechatMessageSettingRepository.GetAsync(x => true)
        );

        return set ?? throw new GirvsException(568, "未找到对应的设置");
    }

    [CapSubscribe(nameof(SendWechatNotificationMessageEvent))]
    public override async Task Handle(SendWechatNotificationMessageEvent @event, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation($"Handling 'SendWechatNotificationMessageEvent' eventId:{@event.Id}");

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync(@event.Id.ToString(), timeSpan, async () =>
        {
            var notificationMessageType = @event.EventNotificationMessageType;
            var receivingChannel = ConvertEventReceivingChannel(@event.EventReceivingChannel);

            MessageTemplate messageTemplate = null;
            NotificationMessageSendState result;
            string message;

            try
            {
                var setting = await GetWechatMessageSetting();
                messageTemplate = GetMessageTemplate(setting, notificationMessageType);
                _sender.Parameter = @event.Parameter;
                _sender.RegisteredUser = new RegisteredUser { OpenId = @event.PhoneNumber };
                _sender.Setting = setting;
                (result, message) = await _sender.SendAsync(messageTemplate);
            }
            catch (Exception e)
            {
                result = NotificationMessageSendState.SendFail;
                message = e.Message;
            }

            var parameterContent = ParameterContent(@event.Parameter.Values);

            var command = new SendNotificationMessageCommand(
                parameterContent,
                messageTemplate?.Desc,
                receivingChannel,
                result,
                notificationMessageType,
                messageTemplate?.TemplateId,
                message,
                @event.PhoneNumber,
                true,
                @event.IdCard
            );

            await _bus.SendCommand(command, cancellationToken);

            if (_notifications.HasNotifications())
            {
                var errorMessage = _notifications.GetNotificationMessage();
                _logger.LogError($"Handling 'SendWechatNotificationMessageEvent' event Error Code:500,Message:{errorMessage}", @event);
            }
        });
    }
}