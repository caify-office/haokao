using Girvs.AuthorizePermission;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Domain.CacheKeyManager;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

/// <summary>
/// 直播管理员接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveAdministratorWebService(
    ILiveAdministratorRepository repository
      , IStaticCacheManager staticCacheManager) : ILiveAdministratorWebService
{
    #region 初始参数

    private readonly ILiveAdministratorRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));

    #endregion

    #region 服务方法

    /// <summary>
    /// 是否是直播管理员
    /// </summary>
    [HttpGet]
    public async Task<bool> IsLiveAdmin()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var result = await _staticCacheManager.GetAsync(LiveAdministratorCacheKeyManager.CreateAdminCacheKey(userId), async () => { return await _repository.ExistEntityAsync(x => x.UserId == userId); });
        return result;
    }

    #endregion
}