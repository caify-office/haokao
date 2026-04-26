using HaoKao.BasicService.Domain.Entities;
using HaoKao.Common;
using IAuthorizationService = HaoKao.BasicService.Application.Interfaces.IAuthorizationService;

namespace HaoKao.BasicService.Application.Services;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class AuthorizationService(
    IStaticCacheManager cacheManager,
    IServiceDataRuleRepository serviceDataRuleRepository,
    IServicePermissionRepository servicePermissionRepository
) : IAuthorizationService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IServiceDataRuleRepository _serviceDataRuleRepository = serviceDataRuleRepository ?? throw new ArgumentNullException(nameof(serviceDataRuleRepository));
    private readonly IServicePermissionRepository _servicePermissionRepository = servicePermissionRepository ?? throw new ArgumentNullException(nameof(servicePermissionRepository));

    /// <summary>
    /// 获取需要授权的功能列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<AuthorizePermissionModel>> GetFunctionOperateList()
    {
        var currentUser = EngineContext.Current.GetCurrentUser();

        if (currentUser == null)
        {
            throw new GirvsException(StatusCodes.Status401Unauthorized, "未授权");
        }

        var currentUserType = currentUser.UserType;

        var availableAuthorizePermissionList = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ServicePermission>.AllCacheKey.Create(),
            () => _servicePermissionRepository.GetAllAsync());

        var currentAvailableAuthorizePermissionList = availableAuthorizePermissionList
                                                      .Where(x => x.OperationPermissions
                                                                   .Any(s => (s.UserType & currentUserType) == currentUserType))
                                                      .ToList();

        var resultPermissions = new List<AuthorizePermissionModel>();
        foreach (var permissionModel in currentAvailableAuthorizePermissionList)
        {
            var permissions = new Dictionary<string, string>();
            var operationPermissions =
                permissionModel.OperationPermissions.Where(x => (x.UserType & currentUserType) == currentUserType);
            foreach (var operationPermission in operationPermissions)
            {
                if (!permissions.ContainsKey(operationPermission.OperationName))
                {
                    permissions.Add(operationPermission.OperationName, operationPermission.Permission.ToString());
                }
            }

            resultPermissions.Add(new AuthorizePermissionModel(
                                      permissionModel.ServiceName,
                                      permissionModel.ServiceId,
                                      permissionModel.Tag,
                                      permissionModel.Order,
                                      permissionModel.FuncModule,
                                      null,
                                      null,
                                      permissions
                                  ));
        }

        var currentSystemModules = EngineContext.Current.ClaimManager.IdentityClaim.SystemModule;

        return resultPermissions
               .Where(x => (x.SystemModule & currentSystemModules) > 0)
               .OrderBy(x => x.Tag)
               .ThenBy(x => x.Order)
               .ToList();
    }

    /// <summary>
    /// 获取需要授权的数据规则列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<AuthorizeDataRuleModel>> GetDataRuleList()
    {
        var currentUser = EngineContext.Current.GetCurrentUser();

        if (currentUser == null)
        {
            throw new GirvsException(StatusCodes.Status401Unauthorized, "未授权");
        }

        var currentUserType = currentUser.UserType;

        var availableAuthorizeDataRuleList = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ServiceDataRule>.AllCacheKey.Create(),
            () => _serviceDataRuleRepository.GetAllAsync());

        var result = new List<AuthorizeDataRuleModel>();

        foreach (var authorizeDataRule in availableAuthorizeDataRuleList.Where(x => currentUserType == (x.UserType & currentUserType)))
        {
            var existReturnResult = result.FirstOrDefault(x => x.EntityTypeName == authorizeDataRule.EntityTypeName);
            if (existReturnResult != null)
            {
                existReturnResult.AuthorizeDataRuleFieldModels.Add(
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
                existReturnResult =
                    new AuthorizeDataRuleModel(
                        authorizeDataRule.EntityDesc,
                        authorizeDataRule.EntityTypeName,
                        authorizeDataRule.Tag,
                        authorizeDataRule.Order,
                        []
                    );

                existReturnResult.AuthorizeDataRuleFieldModels.Add(
                    new AuthorizeDataRuleFieldModel(
                        UserType.All,
                        authorizeDataRule.FieldName,
                        authorizeDataRule.FieldDesc,
                        authorizeDataRule.FieldType,
                        authorizeDataRule.FieldValue,
                        authorizeDataRule.ExpressionType
                    ));

                result.Add(existReturnResult);
            }
        }

        return result.OrderBy(x => x.Order).ToList();
    }

    /// <summary>
    /// 初始化本模块的权限值
    /// </summary>
    [HttpGet]
    public async Task InitAuthorization()
    {
        var eventBus = EngineContext.Current.Resolve<IEventBus>();
        if (eventBus != null)
        {
            var permissionService = new GirvsAuthorizePermissionService();
            var authorizePermissionList = await permissionService.GetAuthorizePermissionList();
            var authorizeDataRules = await permissionService.GetAuthorizeDataRuleList();
            var authorizeEvent = new AuthorizeEvent(authorizeDataRules, authorizePermissionList);
            await eventBus.PublishAsync(authorizeEvent);
        }
    }
}