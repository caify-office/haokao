using HaoKao.Common;
using HaoKao.NotificationMessageService.Domain.Commands.WechatMessageSetting;

namespace HaoKao.NotificationMessageService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "微信配置",
    "0ce02d6a-919f-4bf9-a3c4-aadc1f69933a",
    "32",
    SystemModule.All,
    1
)]
public class WechatMessageSettingService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRepository<WechatMessageSetting> repository)
    : IWechatMessageSettingService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IRepository<WechatMessageSetting> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 获取当前考试的签名设置
    /// </summary>
    /// <returns></returns>
    //[ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    [HttpGet]
    public async Task<EditWechatMessageSettingViewModel> GetCurrentWechatMessageSetting()
    {
        var tenantId = Guid.Empty.ToString();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<WechatMessageSetting>.ByIdCacheKey.Create(tenantId),
            async () => await _repository.GetAsync(x => true));

        if (set == null)
        {
            throw new GirvsException(568, "未找到对应的设置");
        }

        return set.MapToDto<EditWechatMessageSettingViewModel>();
    }

    /// <summary>
    /// 保存当前考试的签名设置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    [HttpPut]
    public async Task UpdateCurrentWechatMessageSetting(EditWechatMessageSettingViewModel model)
    {
        var command = new SaveWechatMessageSettingCommand(
            model.AppId,
            model.AppSecret,
            model.Templates
        );
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}