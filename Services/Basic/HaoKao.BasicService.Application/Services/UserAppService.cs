using Girvs.BusinessBasis.Entities;
using HaoKao.BasicService.Application.Interfaces;
using HaoKao.BasicService.Domain.Entities;
using HaoKao.Common;
using IAuthorizationService = HaoKao.BasicService.Application.Interfaces.IAuthorizationService;

namespace HaoKao.BasicService.Application.Services;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "用户管理",
    "587752d1-7937-4e6a-a035-ee013e58b99b",
    "32",
    SystemModule.All,
    2
)]
public class UserAppService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IUserRepository userRepository,
    ILogger<UserAppService> logger,
    IPermissionRepository permissionRepository,
    IAuthorizationService authorizationService)
    : IUserAppService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly ILogger<UserAppService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPermissionRepository _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
    private readonly IAuthorizationService _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

    /// <summary>
    /// 根据Id获取用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<UserDetailViewModel> GetAsync(Guid id)
    {
        var user = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<User>.ByIdCacheKey.Create(id.ToString()),
            async () => await _userRepository.GetByIdAsync(id));

        if (user == null) throw new GirvsException("未找到对应的用户", StatusCodes.Status404NotFound);


        return user.MapToDto<UserDetailViewModel>();
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post)]
    public async Task<UserEditViewModel> CreateAsync([FromBody] UserEditViewModel model)
    {
        var currentUser = EngineContext.Current.GetCurrentUser();
        if (currentUser == null)
        {
            throw new GirvsException(StatusCodes.Status401Unauthorized, "未登录");
        }

        //如果超级管理员创建的用户，则是特殊用户，如果是租户管理创建的用户，则是普通用户
        var targetUserType =
            currentUser.UserType == UserType.AdminUser ? UserType.SpecialUser : UserType.GeneralUser;

        var command = new CreateUserCommand(
            model.UserAccount,
            model.UserPassword.ToMd5(),
            model.UserName,
            model.ContactNumber,
            model.State,
            targetUserType,
            null,
            ""
        );

        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit)]
    public async Task<UserEditViewModel> UpdateAsync(Guid id, [FromBody] UserEditViewModel model)
    {
        var command = new UpdateUserCommand(
            model.Id.Value,
            model.UserPassword.ToMd5(),
            model.UserName,
            model.ContactNumber,
            model.State,
            model.UserType
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var command = new DeleteUserCommand(id);
        await _bus.SendCommand(command);
    }

    /// <summary>
    /// 根据查询条件获取用户
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.AdminUser | UserType.TenantAdminUser | UserType.SpecialUser | UserType.GeneralUser)]
    public async Task<UserQueryViewModel> GetAsync(UserQueryViewModel queryModel)
    {
        var query = queryModel.MapToQuery<UserQuery>();
        query.OrderBy = nameof(IIncludeCreateTime.CreateTime);
        await _userRepository.GetByQueryAsync(query);
        //修改功能：admin 不能在任何用户列表中返回
        query.Result = query.Result.Where(x => x.UserAccount != "admin").ToList();
        return query.MapToQueryDto<UserQueryViewModel, User>();
    }

    /// <summary>
    /// 获取指定用户的权限
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<AuthorizeModel> GetCurrentUserAuthorization(Guid userId)
    {
        var allAuthorizePermissions = (await _authorizationService.GetFunctionOperateList()).ToList();
        var dicAllAuthorizePermissions = allAuthorizePermissions.ToDictionary(item => item.ServiceId);
        var allAuthorizeDataRules = (await _authorizationService.GetDataRuleList()).ToList();
        var user = await _userRepository.GetUserByIdIncludeRoleAndDataRule(userId);
        //如果当前用户类型为管理员或者租户管理员，则直接返回
        if (user.UserType is UserType.AdminUser or UserType.TenantAdminUser)
        {
            return new AuthorizeModel(allAuthorizeDataRules, allAuthorizePermissions);
        }

        var currentUserRole = user.Roles.Select(x => x.Id).ToArray();
        var userBasalPermissions = await _permissionRepository.GetUserPermissionLimit(user.Id);
        var roleBasalPermissions = await _permissionRepository.GetRoleListPermissionLimit(currentUserRole);
        var mergeBasalPermissions = userBasalPermissions.Union(roleBasalPermissions).ToList();
        mergeBasalPermissions = PermissionHelper.MergeValidateObjectTypePermission(mergeBasalPermissions);


        var permissionViewModels =
            mergeBasalPermissions
                .Where(p => allAuthorizePermissions
                           .Any(a => a.ServiceId == p.AppliedObjectId)
                )
                .Select(p =>
                {
                    var currentServicePermission = allAuthorizePermissions
                        .FirstOrDefault(x => x.ServiceId == p.AppliedObjectId);
                    if (currentServicePermission != null)
                    {
                        var convertPermissionList = PermissionHelper.ConvertPermissionToString(p);
                        var permissions = new Dictionary<string, string>();

                        foreach (var keyValue in convertPermissionList)
                        {
                            foreach (var keyValuePair in currentServicePermission.Permissions
                                                                                 .Where(keyValuePair => keyValuePair.Value == keyValue))
                            {
                                permissions.TryAdd(keyValuePair.Key, keyValue);
                            }
                        }

                        return new AuthorizePermissionModel(
                            currentServicePermission.ServiceName,
                            p.AppliedObjectId,
                            dicAllAuthorizePermissions[p.AppliedObjectId].Tag,
                            0,
                            dicAllAuthorizePermissions[p.AppliedObjectId].SystemModule,
                            null,
                            null,
                            permissions
                        );
                    }

                    return new AuthorizePermissionModel(string.Empty, Guid.Empty, String.Empty, 0, SystemModule.All,
                                                        null, null, null);
                }).ToList();

        var authorizeDataRuleModels = PermissionHelper.ConvertAuthorizeDataRuleModels(user.RulesList);

        //如果是租户管理员，或者普通用户，则根据相对应的考试子模块功能，加载对应的功能菜单列表
        // if (user.UserType is UserType.TenantAdminUser or UserType.GeneralUser)
        // {
        //     var systemModule = EngineContext.Current.ClaimManager.IdentityClaim.SystemModule;
        //     permissionViewModels = permissionViewModels.Where(x => systemModule.HasFlag(x.SystemModule)).ToList();
        // }
        return new AuthorizeModel(authorizeDataRuleModels, permissionViewModels);
    }

    /// <summary>
    /// 添加用户角色操作
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("{userId:guid}/Role")]
    [ServiceMethodPermissionDescriptor("角色管理", Permission.Post_Extend1)]
    public async Task AddUserRole(Guid userId, [FromBody] EditUserRoleViewModel model)
    {
        var command = new AddUserRoleCommand(
            userId,
            model.RoleIds
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 删除用户角色操作
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpDelete("{userId:guid}/Role")]
    [ServiceMethodPermissionDescriptor("角色管理", Permission.Post_Extend1)]
    public async Task DeleteUserRole(Guid userId, [FromBody] EditUserRoleViewModel model)
    {
        var command = new DeleteUserRoleCommand(
            userId,
            model.RoleIds
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPatch("{userId:guid}")]
    [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Edit_Extend1)]
    public async Task Enable(Guid userId)
    {
        var command = new ChangeUserStateCommand(userId, DataState.Enable);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 禁用用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPatch("{userId:guid}")]
    [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Edit_Extend1)]
    public async Task Disable(Guid userId)
    {
        var command = new ChangeUserStateCommand(userId, DataState.Disable);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch("{userId}")]
    [ServiceMethodPermissionDescriptor("重置密码", Permission.Edit_Extend2)]
    public async Task ChangeUserPassword(Guid userId, [FromBody] ChangeUserPassworkViewModel model)
    {
        var command = new ChangeUserPasswordCommand(userId, model.NewPassword);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 用户自行修改密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task UserEditPassword([FromBody] UserEditPasswordViewModel model)
    {
        var currentUserId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var command =
            new UserEditPasswordCommand(currentUserId, model.OldPassword, model.NewPassword);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取用户所包含的角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<List<RoleListViewModel>> GetUserRoles(Guid userId)
    {
        var user = await _userRepository.GetUserByIdIncludeRolesAsync(userId);
        if (user == null)
        {
            throw new GirvsException(StatusCodes.Status404NotFound, "未找到相应数据");
        }

        return user.Roles.MapTo<List<RoleListViewModel>>();
    }

    /// <summary>
    /// 判断登陆名称是否已存在
    /// </summary>
    /// <param name="account"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<bool> ExistAccount(string account, Guid tenantId)
    {
        var user = await _userRepository.GetUserByLoginNameAndTenantIdAsync(account, tenantId);
        return user != null;
    }

    /// <summary>
    /// 获取当前用户的权限、包含功能菜单以及数据权限
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<AuthorizeModel> GetCurrentUserAuthorization()
    {
        var currentUserId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return GetCurrentUserAuthorization(currentUserId);
    }

    /// <summary>
    /// 获取指定用户的数据权限
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    [ServiceMethodPermissionDescriptor("数据配置", Permission.Catalog_Read,
                                       UserType.GeneralUser | UserType.TenantAdminUser)]
    public async Task<List<UserDataRuleListViewModel>> GetUserDataRule(Guid userId)
    {
        var user = await _userRepository.GetUserByIdIncludeRoleAndDataRule(userId);
        if (user == null) throw new GirvsException("未找到对应的用户", StatusCodes.Status404NotFound);


        var rulesList = user.RulesList;
        var authorizeDataRuleModels = new List<UserDataRuleListViewModel>();

        if (rulesList == null || !rulesList.Any()) return authorizeDataRuleModels;

        foreach (var entityTypeItem in rulesList.GroupBy(x => x.EntityTypeName))
        {
            var entityType = new UserDataRuleListViewModel
            {
                EntityTypeName = entityTypeItem.Key,
                //EntityDesc = authorizeDataRule.EntityDesc
            };

            foreach (var field in entityTypeItem)
            {
                entityType.DataRuleListFieldViewModels.Add(new UserDataRuleListFieldViewModel
                {
                    ExpressionType = field.ExpressionType,
                    FieldType = field.FieldType,
                    FieldName = field.FieldName,
                    FieldValue = field.FieldValue,
                    FieldDesc = field.FieldDesc,
                    FieldValueText = field.FieldValueText
                });
            }

            authorizeDataRuleModels.Add(entityType);
        }

        return authorizeDataRuleModels;
    }

    /// <summary>
    /// 保存指定用户的数据权限
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost("{userId}")]
    [ServiceMethodPermissionDescriptor("数据配置", Permission.Catalog_Read,
                                       UserType.GeneralUser | UserType.TenantAdminUser)]
    public async Task SaveUserDataRule(Guid userId, [FromBody] List<SaveUserDataRuleViewModel> models)
    {
        if (models == null || models.Count == 0)
        {
            _logger.LogDebug("没有接到任何要保存的数据，默认为清除所有的数据规则授权");
        }
        else
        {
            _logger.LogDebug($"接到要保存的数据条数为：{models.Count}");
            _logger.LogDebug($"接到要保存的数据内容：{System.Text.Json.JsonSerializer.Serialize(models)}");
        }

        var userRules = models.Select(dataRuleFieldModel => new UserRule
                              {
                                  EntityTypeName = dataRuleFieldModel.EntityTypeName,
                                  EntityDesc = dataRuleFieldModel.EntityDesc,
                                  FieldName = dataRuleFieldModel.FieldName,
                                  FieldDesc = dataRuleFieldModel.FieldDesc,
                                  FieldType = dataRuleFieldModel.FieldType,
                                  FieldValue = dataRuleFieldModel.FieldValue,
                                  FieldValueText = dataRuleFieldModel.FieldValueText,
                                  ExpressionType = dataRuleFieldModel.ExpressionType
                              })
                              .ToList();

        var command = new UpdateUserRuleCommand(userId, userRules);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}