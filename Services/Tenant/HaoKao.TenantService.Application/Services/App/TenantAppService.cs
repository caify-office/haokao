using HaoKao.Common;
using HaoKao.TenantService.Application.Services.Management;
using HaoKao.TenantService.Application.ViewModels;
using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Enums;
using HaoKao.TenantService.Domain.Repositories;
using System.Collections.Generic;

namespace HaoKao.TenantService.Application.Services.App;

/// <summary>
/// 租户App端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class TenantAppService(
    ITenantService service,
    ITenantRepository repository,
    IStaticCacheManager cacheManager
) : ITenantAppService
{
    /// <summary>
    /// 获取租户列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<TenantQueryListViewModel>> Get()
    {
        var query = new TenantQueryViewModel
        {
            //指定获取经济师的相关考试级别
            PageIndex = 1,
            PageSize = int.MaxValue,
            StartState = EnableState.Enable,
        };

        var result = await service.GetAsync(query);

        return result.Result;
    }

    /// <summary>
    /// 获取当前租户详情
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<TenantQueryListViewModel> GetCurrent()
    {
        var tenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>();
        var tenant = await repository.GetByIdAsync(tenantId);
        return tenant.MapToDto<TenantQueryListViewModel>();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [AllowAnonymous]
    public async Task<TenantQueryListViewModel> GetAsync(Guid id)
    {
        var result = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Tenant>.ByIdCacheKey.Create(id.ToString()),
            () => repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("未找到对应的数据");

        return result.MapToDto<TenantQueryListViewModel>();
    }
}