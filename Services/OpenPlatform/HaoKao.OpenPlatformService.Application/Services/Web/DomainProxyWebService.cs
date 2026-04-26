using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.ViewModels.DomainProxy;
using HaoKao.OpenPlatformService.Domain.Extensions;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

/// <summary>
/// 域名服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[AllowAnonymous]
public class DomainProxyWebService(
    IDomainProxyRepository repository,
    IStaticCacheManager staticCacheManager) : IDomainProxyWebService
{
    private readonly IDomainProxyRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));

    /// <summary>
    /// 获取指定域名的租户相关设置
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BrowseDomainProxyViewModel> GetCurrentDomainProxy(string domain)
    {
        var cacheKey = domain.CreateDomainProxyCacheKey();
        var result = await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var domainProxy = await _repository.GetByDomain(domain);
            if (domainProxy == null)
            {
                throw new GirvsException($"未知的域名：{domain}", StatusCodes.Status404NotFound);
            }

            return new BrowseDomainProxyViewModel
            {
                Id = default,
                Domain = domainProxy.Domain,
                TenantId = domainProxy.TenantId,
                TenantName = domainProxy.TenantName,
                ClientId = domainProxy.AccessClient.ClientId,
                ClientName = domainProxy.AccessClient.ClientName,
                Description = domainProxy.AccessClient.Description,
                LogoUri = domainProxy.AccessClient.LogoUri
            };
        });

        return result;
    }
}