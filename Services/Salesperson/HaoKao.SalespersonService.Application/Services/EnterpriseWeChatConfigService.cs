using HaoKao.SalespersonService.Application.Interfaces;
using HaoKao.SalespersonService.Application.ViewModels;
using HaoKao.SalespersonService.Domain.Commands;
using HaoKao.SalespersonService.Domain.Repositories;

namespace HaoKao.SalespersonService.Application.Services;

/// <summary>
/// 企业微信配置管理端服务接口
/// </summary>
/// <param name="bus"></param>
/// <param name="cacheManager"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "企业微信配置",
    "d4df196e-cbff-4179-8303-834c9a18f39d",
    "32",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class EnterpriseWeChatConfigService(
    IMediatorHandler bus,
    IStaticCacheManager cacheManager,
    INotificationHandler<DomainNotification> notifications,
    IEnterpriseWeChatConfigRepository repository
) : IEnterpriseWeChatConfigService
{
    #region 私有参数

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IEnterpriseWeChatConfigRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据租户获取
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseEnterpriseWeChatConfigViewModel> Get()
    {
        var tenentId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
        var entity = await _repository.GetAsync(x => x.TenantId == tenentId)
            ?? throw new GirvsException("对应的企业微信配置不存在", StatusCodes.Status404NotFound);

        return entity.MapToDto<BrowseEnterpriseWeChatConfigViewModel>();
    }

    /// <summary>
    /// 保存企业微信配置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("保存", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateEnterpriseWeChatConfigViewModel model)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
        var config = await _repository.GetAsync(x => x.TenantId == tenantId);
        if (config == null)
        {
            var command = model.MapToCommand<CreateEnterpriseWeChatConfigCommand>();
            await _bus.TrySendCommand(command, _notifications);
        }
        else
        {
            var command = new UpdateEnterpriseWeChatConfigCommand(config.Id, model.CorpId, model.CorpName, model.CorpSecret);
            await _bus.TrySendCommand(command, _notifications);
        }
    }

    #endregion
}