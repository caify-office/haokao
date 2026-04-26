using HaoKao.Common;
using HaoKao.NotificationMessageService.Domain.Commands.MobileMessageSetting;

namespace HaoKao.NotificationMessageService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "短信配置",
    "92531b00-88e1-47ce-8ad3-9e4bb76c645f",
    "32",
    SystemModule.All,
    1
)]
public class MobileMessageSettingService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRepository<MobileMessageSetting> repository)
    : IMobileMessageSettingService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IRepository<MobileMessageSetting> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 获取当前考试的签名设置
    /// </summary>
    /// <returns></returns>
    //[ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    //[HttpGet]
    public async Task<EditMobileMessageSettingViewModel> GetCurrentMobileMessageSetting()
    {
        var tenantId = Guid.Empty.ToString();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<MobileMessageSetting>.ByIdCacheKey.Create(tenantId),
            async () => await _repository.GetAsync(x => true));

        if (set == null)
        {
            throw new GirvsException(568, "未找到对应的设置");
        }

        return set.MapToDto<EditMobileMessageSettingViewModel>();
    }

    /// <summary>
    /// 保存当前考试的签名设置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    [HttpPut]
    public async Task UpdateCurrentMobileMessageSetting(EditMobileMessageSettingViewModel model)
    {
        var command = new SaveMobileMessageSettingCommand(
            model.MobileMessagePlatform,
            model.AppId,
            model.AppSecret,
            model.SignList,
            model.DefaultSign,
            model.Templates,
            model.SmsSdkAppId
        );
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}