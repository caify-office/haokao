using HaoKao.NotificationMessageService.Domain.MessageSenders;

namespace HaoKao.NotificationMessageService.Application.EventHandlers;

public class SendMobileNotificationMessageEventHandler(
    IServiceProvider serviceProvider,
    ILocker locker,
    IStaticCacheManager cacheManager,
    ILogger<SendMobileNotificationMessageEventHandler> logger,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IMobileMessageSender mobileMessageSender)
    :
        SendNotificationMessageAbstractEventHandler<SendMobileNotificationMessageEvent>(serviceProvider, cacheManager)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly ILogger<SendMobileNotificationMessageEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));
    private readonly IMobileMessageSender _mobileMessageSender = mobileMessageSender ?? throw new ArgumentNullException(nameof(mobileMessageSender));

    private async Task<MobileMessageSetting> GetMobileMessageSetting()
    {
        var mobileMessageSettingRepository = EngineContext.Current.Resolve<IRepository<MobileMessageSetting>>();
        var tenantId = Guid.Empty.ToString();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<MobileMessageSetting>.ByIdCacheKey.Create(tenantId),
            () => mobileMessageSettingRepository.GetAsync(x => true)
        );

        return set ?? throw new GirvsException(568, "未找到对应的设置");
    }

    private async Task<TenantSignSetting> GetTenantSignSetting(string defaultSign)
    {
        // 如果没有请求头，默认为无登陆用户所发消息
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        if (tenantId.IsNullOrEmpty())
        {
            return new TenantSignSetting
            {
                TenantId = Guid.Empty,
                Sign = defaultSign
            };
        }

        var tenantSignSettingRepository = EngineContext.Current.Resolve<IRepository<TenantSignSetting>>();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<TenantSignSetting>.ByIdCacheKey.Create(tenantId),
            () => tenantSignSettingRepository.GetAsync(x => x.TenantId.ToString() == tenantId));

        return set ?? new TenantSignSetting
        {
            TenantId = Guid.Empty,
            Sign = defaultSign
        };
    }

    [CapSubscribe(nameof(SendMobileNotificationMessageEvent))]
    public override async Task Handle(SendMobileNotificationMessageEvent @event, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation($"Handling 'SendMobileNotificationMessageEvent' eventId:{@event.Id}");

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync(@event.Id.ToString(), timeSpan, async () =>
        {
            var notificationMessageType = @event.EventNotificationMessageType;

            MessageTemplate messageTemplate = null;
            NotificationMessageSendState result;
            string message;

            try
            {
                var setting = await GetMobileMessageSetting();
                messageTemplate = GetMessageTemplate(setting, notificationMessageType);
                _mobileMessageSender.Parameter = @event.Parameter;
                _mobileMessageSender.PhoneNumber = @event.PhoneNumber;
                _mobileMessageSender.Setting = setting;
                _mobileMessageSender.TenantSignSetting = await GetTenantSignSetting(setting.DefaultSign);
                (result, message) = await _mobileMessageSender.SendAsync(messageTemplate);
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
                ReceivingChannel.Mobile,
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
                _logger.LogError($"Handling 'SendMobileNotificationMessageEvent' event Error Code:500,Message:{errorMessage}", @event);
            }
        });
    }
}