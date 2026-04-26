using HaoKao.Common.AuthorizeRepositories;

namespace HaoKao.DrawPrizeService.Infrastructure.AuthorizeDataSource;

public class AuthorizeRepository(
    IAuthorityNetWorkRepository authorityNetWorkRepository,
    IStaticCacheManager staticCacheManager) : MyGirvsAuthorizeCompare
{
    private readonly IAuthorityNetWorkRepository _authorityNetWorkRepository = authorityNetWorkRepository ??
                                                                               throw new ArgumentNullException(nameof(authorityNetWorkRepository));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));

    public override AuthorizeModel GetCurrnetUserAuthorize()
    {
        var key =
            $"{GirvsAuthorizePermissionCacheKeyManager.CurrentUserAuthorizeCacheKeyPrefix}:{EngineContext.Current.ClaimManager?.IdentityClaim.UserId??"临时用户"}";

        var cacheKey = new CacheKey(key).Create();
        var authorize = _staticCacheManager.Get(
            cacheKey,
            () => _authorityNetWorkRepository.GetUserAuthorization().Result);

        //由于Refit调用存在一定的问题，临时解决方案
        if (authorize == null || authorize.AuthorizePermissions.Count == 0 ||
            authorize.AuthorizeDataRules.Count == 0)
        {
            authorize = _authorityNetWorkRepository.GetUserAuthorization().Result;
            _staticCacheManager.SetAsync(cacheKey, authorize).Wait();
        }

        return authorize ??
               new AuthorizeModel([], []);
    }
    public override bool ContainsPublicData { get; set; } = true;
}