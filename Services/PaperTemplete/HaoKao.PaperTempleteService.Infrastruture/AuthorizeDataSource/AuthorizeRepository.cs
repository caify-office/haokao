

//using Girvs.AuthorizePermission;
//using Girvs.AuthorizePermission.AuthorizeCompare;
//using Girvs.Cache.Caching;
//using Girvs.Infrastructure;
//using HaoKao.Common.AuthorizeRepositories;

//namespace HaoKao.PaperTempleteService.Infrastructure.AuthorizeDataSource
//{
//    public class AuthorizeRepository : GirvsAuthorizeCompare
//    {
//        private readonly IAuthorityNetWorkRepository _authorityNetWorkRepository;
//        private readonly IStaticCacheManager _staticCacheManager;

//        public AuthorizeRepository(
//            IAuthorityNetWorkRepository authorityNetWorkRepository,
//            IStaticCacheManager staticCacheManager
//        )
//        {
//            _authorityNetWorkRepository = authorityNetWorkRepository ??
//                                          throw new ArgumentNullException(nameof(authorityNetWorkRepository));
//            _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
//        }

//        public override AuthorizeModel GetCurrnetUserAuthorize()
//        {
//            var key =
//                $"{GirvsAuthorizePermissionCacheKeyManager.CurrentUserAuthorizeCacheKeyPrefix}:{EngineContext.Current.ClaimManager.IdentityClaim.UserId}";

//            var cacheKey = new CacheKey(key).Create();
//            var authorize = _staticCacheManager.Get(
//                cacheKey,
//                () => _authorityNetWorkRepository.GetUserAuthorization().Result);

//            //由于Refit调用存在一定的问题，临时解决方案
//            if (authorize == null || authorize.AuthorizePermissions.Count == 0 ||
//                authorize.AuthorizeDataRules.Count == 0)
//            {
//                authorize = _authorityNetWorkRepository.GetUserAuthorization().Result;
//                _staticCacheManager.Set(cacheKey, authorize);
//            }

//            return authorize ??
//                   new AuthorizeModel(new List<AuthorizeDataRuleModel>(), new List<AuthorizePermissionModel>());
//        }
//    }
//}