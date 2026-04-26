using HaoKao.Common;

namespace HaoKao.NotificationMessageService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "站内消息配置",
    "e44bc5c5-06aa-487b-a144-81b456213cae",
    "32",
    SystemModule.All,
    1
)]
public class SiteMessageSettingService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRepository<InSiteMessageSetting> repository)
    : ISiteMessageSettingService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IRepository<InSiteMessageSetting> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 获取站内消息模板设置
    /// </summary>
    /// <returns></returns>
    //[ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser|UserType.TenantAdminUser|UserType.GeneralUser)]
    [HttpGet]
    public async Task<List<MessageTemplate>> GetCurrentMobileMessageSetting()
    {
        var tenantId = Guid.Empty.ToString();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<InSiteMessageSetting>.ByIdCacheKey.Create(tenantId),
            async () => await _repository.GetAsync(x => true));

        if (set == null)
        {
            throw new GirvsException(568, "未找到对应的设置");
        }

        return set.Templates;
    }

    /// <summary>
    /// 保存站内消息模板设置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    [HttpPut]
    public async Task UpdateCurrentMobileMessageSetting(List<MessageTemplate> model)
    {
        var command = new SaveInSiteMessageSettingCommand(model
        );
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}