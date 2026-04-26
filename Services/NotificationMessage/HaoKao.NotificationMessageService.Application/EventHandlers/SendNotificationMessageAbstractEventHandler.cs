namespace HaoKao.NotificationMessageService.Application.EventHandlers;

public abstract class SendNotificationMessageAbstractEventHandler<TEvent>(
    IServiceProvider serviceProvider,
    IStaticCacheManager staticCacheManager) : GirvsIntegrationEventHandler<TEvent>(serviceProvider)
    where TEvent : SendNotificationMessageEvent
{
    private readonly IStaticCacheManager _cacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));

    #region ctor

    #endregion

    protected virtual MessageTemplate GetMessageTemplate(MessageSetting setting, EventNotificationMessageType notificationMessageType)
    {
        if (setting.Templates != null)
        {
            return setting.Templates.FirstOrDefault(x => x.NotificationMessageType == notificationMessageType);
        }

        throw new GirvsException("未找到对应的模板");
    }

    /// <summary>
    /// 参数内容化
    /// </summary>
    /// <param name="ps"></param>
    /// <returns></returns>
    protected virtual string ParameterContent(IEnumerable<string> ps)
    {
        if (ps == null || !ps.Any())
        {
            return string.Empty;
        }

        return string.Join(',', ps);
    }

    
    protected ReceivingChannel ConvertEventReceivingChannel(EventReceivingChannel eventReceivingChannel)
    {
        return (ReceivingChannel)(int)eventReceivingChannel;
    }

    /// <summary>
    /// 获取接收对象
    /// </summary>
    /// <param name="idCard"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    protected async Task<RegisteredUser> GetRegisteredUser(string idCard, string phoneNumber)
    {
        var registeredUserRepository = EngineContext.Current.Resolve<IRegisteredUserRepository>();
        var cacheKey = new CacheKey(nameof(RegisteredUser) + ":{0}").Create(idCard, phoneNumber);
        var registerUser = await _cacheManager.GetAsync(cacheKey, () => registeredUserRepository.GetRegisteredUser(idCard, phoneNumber));
        // var registerUser = await registeredUserRepository.GetRegisteredUser(idCard, phoneNumber);
        if (registerUser == null)
        {
            return new RegisteredUser
            {
                CardId = idCard,
                ContactNumber = phoneNumber,
            };
        }

        return registerUser;
    }

    protected string BuilderReceiver(RegisteredUser user, string phoneNumber)
    {
        if (user == null || string.IsNullOrEmpty(user.UserName))
        {
            return phoneNumber;
        }

        return $"{user.UserName}({user.ContactNumber})";
    }
}