using HaoKao.DeepSeekService.Domain;

namespace HaoKao.DeepSeekService.Application.Services;

public interface IDeepSeekConfigService : IAppWebApiService, IManager
{
    /// <summary>
    /// 按租户获取DeepSeek配置
    /// </summary>
    /// <returns></returns>
    Task<DeepSeekConfig> Get();

    /// <summary>
    /// 保存DeepSeek配置
    /// </summary>
    /// <returns></returns>
    Task Update(DeepSeekConfig model);
}

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "DeepSeek配置",
    "06879fd4-fd14-7888-8000-63343b47bcbb",
    "32",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class DeepSeekConfigService(
    IDeepSeekConfigRepository repository,
    IStaticCacheManager cacheManager
) : IDeepSeekConfigService
{
    /// <summary>
    /// 保存DeepSeek配置
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<DeepSeekConfig> Get()
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
        var entity = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<DeepSeekConfig>.ByTenantKey.Create(),
            () => repository.GetByTenantId(tenantId)
        ) ?? new DeepSeekConfig(Guid.NewGuid(), tenantId);
        return entity;
    }

    /// <summary>
    /// 按租户获取DeepSeek配置
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("保存", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] DeepSeekConfig model)
    {
        await repository.SaveAsync(model);
        var cacheKey = GirvsEntityCacheDefaults<DeepSeekConfig>.ByTenantKey.Create();
        await cacheManager.SetAsync(cacheKey, model);
    }
}