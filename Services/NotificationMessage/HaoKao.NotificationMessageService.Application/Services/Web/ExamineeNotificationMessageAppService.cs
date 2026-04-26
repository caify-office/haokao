using HaoKao.Common;

namespace HaoKao.NotificationMessageService.Application.Services.Web;

/// <summary>
/// 考生端-消息管理相关API
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Route("/api/examinee/message")]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ExamineeNotificationMessageAppService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    INotificationMessageRepository notificationMessageRepository,
    IRepository<WechatMessageSetting> wechatMessageSettingRepository
) : IExamineeNotificationMessageAppService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly INotificationMessageRepository _notificationMessageRepository = notificationMessageRepository ?? throw new ArgumentNullException(nameof(notificationMessageRepository));
    private readonly IRepository<WechatMessageSetting> _wechatMessageSettingRepository = wechatMessageSettingRepository ?? throw new ArgumentNullException(nameof(wechatMessageSettingRepository));

    /// <summary>
    /// 获取考生的站内消息列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamineeNotificationMessageQueryViewModel> GetByQueryAsync(
        [FromQuery] ExamineeNotificationMessageQueryViewModel queryModel)
    {
        queryModel.ReceivingChannel ??= ReceivingChannel.InSite;
        var query = queryModel.MapToQuery<NotificationMessageQuery>();
        var entityName = nameof(NotificationMessage).ToLowerInvariant();
        var keyStr = $"{entityName}:{query.IdCard.ToMd5()}:{query.GetCacheKey().ToMd5()}";
        // var tempQuery = await _cacheManager.GetAsync(new CacheKey(keyStr).Create(), async () =>
        // {
        //     await _notificationMessageRepository.GetByQueryResultSpAsync(query);
        //     return query;
        // });
        //
        // if (!query.Equals(tempQuery))
        // {
        //     query.RecordCount = tempQuery.RecordCount;
        //     query.Result = tempQuery.Result;
        // }

        await _notificationMessageRepository.GetByQueryResultSpAsync(query);

        var result = query.MapToQueryDto<ExamineeNotificationMessageQueryViewModel, NotificationMessage>();
        return result;
    }

    /// <summary>
    /// 获取考生未读条数
    /// </summary>
    /// <returns></returns>
    [HttpGet("unreadCount")]
    public async Task<int> GetUnreadCount(string idCard, Guid tenantAccessId)
    {
        var dbResult = await _notificationMessageRepository.GetUnReadCountSpAsync(idCard, tenantAccessId);
        return dbResult;
    }

    /// <summary>
    /// 根据OpenId，判断当前时否关注公众号
    /// </summary>
    /// <param name="openId"></param>
    /// <returns></returns>
    [HttpGet("FollowWeChat/{openId}")]
    public async Task<bool> FollowWeChat(string openId)
    {
        var tenantId = Guid.Empty.ToString();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<WechatMessageSetting>.ByIdCacheKey.Create(tenantId),
            async () => await _wechatMessageSettingRepository.GetAsync(x => true));
        return await _notificationMessageRepository.FollowWeChat(set, openId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("OpenId/{code}")]
    public async Task<string> GetOpenIdByCode(string code)
    {
        var tenantId = Guid.Empty.ToString();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<WechatMessageSetting>.ByIdCacheKey.Create(tenantId),
            async () => await _wechatMessageSettingRepository.GetAsync(x => true));
        return await _notificationMessageRepository.GetOpenIdAsync(set, code);
    }

    // /// <summary>
    // /// 设置考生站点消息全部已读 暂时不需要（后续用到再打开）
    // /// </summary>
    // /// <returns></returns>
    //[HttpPatch("readAll")]
    //public async Task ReadAll(string idCard)
    //{
    //    // 目前只有站内消息
    //    var command = new ReadAllSiteNotificationMessageCommand( idCard);
    //    await _bus.SendCommand(command);
    //    if (_notifications.HasNotifications())
    //    {
    //        var errorMessage = _notifications.GetNotificationMessage();
    //        throw new GirvsException(568, errorMessage);
    //    }
    //}

    /// <summary>
    /// 考生阅读指定消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("read/{id}")]
    public async Task ReadMessage(Guid id)
    {
        var command = new ReadSiteNotificationMessageCommand(id);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(568, errorMessage);
        }
    }
}