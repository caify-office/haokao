using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.ViewModels.DomainProxy;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class DomainProxyService(IDomainProxyRepository repository) : IDomainProxyService
{
    /// <summary>
    /// 获取当前租户下的所有代理域名
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    public async Task<List<BrowseDomainProxyViewModel>> GetCurrentDomainProxy()
    {
        var list = await repository.GetCurrentTenantDomainList();
        return list.Select(x => new BrowseDomainProxyViewModel
        {
            Id = x.Id,
            Domain = x.Domain,
            TenantId = x.TenantId,
            TenantName = x.TenantName,
            ClientId = x.AccessClient.ClientId,
            ClientName = x.AccessClient.ClientName,
            Description = x.AccessClient.Description,
            LogoUri = x.AccessClient.LogoUri
        }).ToList();
    }

    [HttpGet]
    public string GetTenantId()
    {
        return EngineContext.Current.ClaimManager.GetTenantId();
    }
}