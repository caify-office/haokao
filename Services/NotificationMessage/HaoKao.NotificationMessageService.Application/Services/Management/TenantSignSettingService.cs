using HaoKao.Common;

namespace HaoKao.NotificationMessageService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "消息配置",
    "0899bc0e-7927-4ce6-b5ca-5bb564b702fe",
    "32",
    SystemModule.All,
    1
)]
public class TenantSignSettingService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRepository<TenantSignSetting> repository,
    IMobileMessageSettingService mobileMessageSettingService)
    : ITenantSignSettingService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IRepository<TenantSignSetting> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMobileMessageSettingService _mobileMessageSettingService = mobileMessageSettingService ??
                                                                                 throw new ArgumentNullException(nameof(mobileMessageSettingService));

    /// <summary>
    /// 读取当前考试的签名设置
    /// </summary>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    [HttpGet]
    public async Task<string> GetCurrentSignSetting()
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        var set = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<TenantSignSetting>.ByIdCacheKey.Create(tenantId),
            async () => await _repository.GetAsync(x => true));

        if (set != null)
        {
            return set.Sign;
        }

        var mSet = await _mobileMessageSettingService.GetCurrentMobileMessageSetting();

        return mSet == null ? string.Empty : mSet.DefaultSign;
    }

    /// <summary>
    /// 保存当前考试的签名设置
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("保存", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    [HttpPut]
    public async Task UpdateCurrentSignSetting(string sign)
    {
        var command = new SaveTenantSignSettingCommand(sign);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取系统签名列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<string>> GetSystemSignList()
    {
        var mobileSetting = await _mobileMessageSettingService.GetCurrentMobileMessageSetting();
        return mobileSetting.SignList;
    }
}