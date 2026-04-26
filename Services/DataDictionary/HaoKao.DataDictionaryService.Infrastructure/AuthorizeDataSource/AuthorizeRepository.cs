namespace HaoKao.DataDictionaryService.Infrastructure.AuthorizeDataSource;

public class AuthorizeRepository(IAuthorityNetWorkRepository repository, IStaticCacheManager cacheManager) : GirvsAuthorizeCompare
{
    private readonly IAuthorityNetWorkRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    public override AuthorizeModel GetCurrnetUserAuthorize()
    {
        var key = $"{GirvsAuthorizePermissionCacheKeyManager.CurrentUserAuthorizeCacheKeyPrefix}:{EngineContext.Current.ClaimManager.IdentityClaim.UserId}";

        var cacheKey = new CacheKey(key).Create();
        var authorize = _cacheManager.Get(cacheKey, () => _repository.GetUserAuthorization().Result);

        //由于Refit调用存在一定的问题，临时解决方案
        if (authorize == null
         || authorize.AuthorizePermissions.Count == 0
         || authorize.AuthorizeDataRules.Count == 0)
        {
            authorize = _repository.GetUserAuthorization().Result;
            _cacheManager.SetAsync(cacheKey, authorize).Wait();
        }

        return authorize ?? new AuthorizeModel([], []);
    }

    public override bool ContainsPublicData { get; set; } = true;
}