using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Store;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

/// <summary>
/// 请求资源服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class ScopeService : IScopeService
{
    [HttpGet]
    public Task<ICollection<string>> Get()
    {
        ICollection<string> list =
        [
            OidcConstants.StandardScopes.OfflineAccess,
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Phone,
            IdentityServerConstants.StandardScopes.Profile,
            HaoKaoResourcesStore.ApiServiceResourceGroupName
        ];
        return Task.FromResult(list);
    }
}