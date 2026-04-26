using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.AuthorizeDataSource;

public class AuthorizeRepository(IStaticCacheManager staticCacheManager) : GirvsAuthorizeCompare
{
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));

    public override AuthorizeModel GetCurrnetUserAuthorize()
    {
        //由于是循环依赖注入，导致此处要解决循环依赖的问题，所以在此处特殊处理。后续可能需要同步进行修改
        //2021-08-25 By xufeng
        if (EngineContext.Current.HttpContext == null || !EngineContext.Current.HttpContext.User.Identity!.IsAuthenticated)
            return new AuthorizeModel([], []);

        var key =
            $"{GirvsAuthorizePermissionCacheKeyManager.CurrentUserAuthorizeCacheKeyPrefix}:{EngineContext.Current.ClaimManager.IdentityClaim.UserId}";

        var authorize = _staticCacheManager.Get(
            new CacheKey(key).Create(), () =>
            {
                var dbContext = EngineContext.Current.Resolve<BasicDbContext>();

                var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();

                var user = dbContext.Users.AsNoTracking().Include(x => x.Roles).Include(x => x.RulesList)
                    .FirstOrDefault(x => x.Id == userId);
                //如果当前用户类型为管理员或者租户管理员，则直接返回
                if (user.UserType is UserType.AdminUser or UserType.TenantAdminUser)
                {
                    var authorizePermissions = GetFunctionOperateList(dbContext, user);
                    var authorizeDataRules = GetDataRuleList(dbContext, user);
                    return new AuthorizeModel(authorizeDataRules, authorizePermissions);
                }

                var currentUserRole = user.Roles.Select(x => x.Id).ToArray();
                var userBasalPermissions = dbContext.BasalPermissions.AsNoTracking().Where(x => x.AppliedId ==
                    userId).ToList();
                ;
                var roleBasalPermissions = dbContext.BasalPermissions.AsNoTracking()
                    .Where(x => currentUserRole.Contains(x.AppliedId)).ToList();
                var mergeBasalPermissions = userBasalPermissions.Union(roleBasalPermissions).ToList();
                mergeBasalPermissions =
                    PermissionHelper.MergeValidateObjectTypePermission(mergeBasalPermissions);


                var permissionViewModels =
                    mergeBasalPermissions.Select(p => new AuthorizePermissionModel(
                        null,
                        p.AppliedObjectId,
                        null,
                        0,
                        SystemModule.All,
                        null,
                        null,
                        PermissionHelper.ConvertPermissionToString(p).ToDictionary(x => x, x => x)
                    )).ToList();

                var authorizeDataRuleModels = PermissionHelper.ConvertAuthorizeDataRuleModels(user.RulesList);

                return new AuthorizeModel(authorizeDataRuleModels, permissionViewModels);
            });

        return authorize ??
               new AuthorizeModel([], []);
    }


    /// <summary>
    /// 获取需要授权的功能列表
    /// </summary>
    /// <returns></returns>
    private List<AuthorizePermissionModel> GetFunctionOperateList(BasicDbContext dbContext,
        User currentUser)
    {
        var currentUserType = currentUser.UserType;

        var availableAuthorizePermissionList = dbContext.ServicePermissions.AsNoTracking().ToList();

        var currentAvailableAuthorizePermissionList = availableAuthorizePermissionList.Where(x =>
            x.OperationPermissions.Any(s => (s.UserType & currentUserType) == currentUserType)).ToList();

        var resultPermissions = new List<AuthorizePermissionModel>();
        foreach (var permissionModel in currentAvailableAuthorizePermissionList)
        {
            var permissions = new Dictionary<string, string>();
            foreach (var operationPermission in permissionModel.OperationPermissions)
            {
                if (!permissions.ContainsKey(operationPermission.OperationName))
                {
                    permissions.Add(operationPermission.OperationName, operationPermission.Permission.ToString());
                }
            }

            resultPermissions.Add(new AuthorizePermissionModel(
                permissionModel.ServiceName,
                permissionModel.ServiceId,
                null,
                0,
                SystemModule.All,
                null,
                null,
                permissions
            ));
        }

        return resultPermissions;
    }

    /// <summary>
    /// 获取需要授权的数据规则列表
    /// </summary>
    /// <returns></returns>
    private List<AuthorizeDataRuleModel> GetDataRuleList(BasicDbContext dbContext, User currentUser)
    {
        if (currentUser == null)
        {
            throw new GirvsException(StatusCodes.Status401Unauthorized, "未授权");
        }

        var currentUserType = currentUser.UserType;

        var availableAuthorizeDataRuleList = dbContext.ServiceDataRules.AsNoTracking().ToList();

        var result = new List<AuthorizeDataRuleModel>();

        foreach (var authorizeDataRule in availableAuthorizeDataRuleList.Where(x =>
                     currentUserType == (x.UserType & currentUserType)))
        {
            var existReturnReuslt =
                result.FirstOrDefault(x => x.EntityTypeName == authorizeDataRule.EntityTypeName);
            if (existReturnReuslt != null)
            {
                existReturnReuslt.AuthorizeDataRuleFieldModels.Add(
                    new AuthorizeDataRuleFieldModel(
                        UserType.All,
                        authorizeDataRule.FieldName,
                        authorizeDataRule.FieldDesc,
                        authorizeDataRule.FieldType,
                        authorizeDataRule.FieldValue,
                        authorizeDataRule.ExpressionType
                    ));
            }
            else
            {
                existReturnReuslt =
                    new AuthorizeDataRuleModel(
                        authorizeDataRule.EntityDesc,
                        authorizeDataRule.EntityTypeName,
                        null,
                        0,
                        []
                    );

                existReturnReuslt.AuthorizeDataRuleFieldModels.Add(
                    new AuthorizeDataRuleFieldModel(
                        UserType.All,
                        authorizeDataRule.FieldName,
                        authorizeDataRule.FieldDesc,
                        authorizeDataRule.FieldType,
                        authorizeDataRule.FieldValue,
                        authorizeDataRule.ExpressionType
                    ));

                result.Add(existReturnReuslt);
            }
        }

        return result;
    }
}