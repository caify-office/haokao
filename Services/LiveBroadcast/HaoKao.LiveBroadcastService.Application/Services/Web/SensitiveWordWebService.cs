using Girvs.AuthorizePermission;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class SensitiveWordWebService(IStaticCacheManager cacheManager, ISensitiveWordRepository repository) : ISensitiveWordWebService
{
    [HttpGet]
    public async Task<BrowseSensitiveWordViewModel> Get()
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
        var sensitiveWord = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<SensitiveWord>.ByTenantKey.Create(),
            () => repository.GetAsync(x => x.TenantId == tenantId)
        ) ?? throw new GirvsException("对应的敏感词不存在", StatusCodes.Status404NotFound);
        return sensitiveWord.MapToDto<BrowseSensitiveWordViewModel>();
    }
}