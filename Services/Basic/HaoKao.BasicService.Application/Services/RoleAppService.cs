using HaoKao.BasicService.Application.Interfaces;
using HaoKao.BasicService.Domain.Entities;
using HaoKao.Common;
using IAuthorizationService = HaoKao.BasicService.Application.Interfaces.IAuthorizationService;

namespace HaoKao.BasicService.Application.Services;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "角色管理",
    "4a4fcf52-7696-47e9-b363-2acdd5735dc8",
    "32",
    SystemModule.All,
    1
)]
public class RoleAppService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository,
    IAuthorizationService authorizationService
) : IRoleAppService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IRoleRepository _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
    private readonly IAuthorizationService _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
    private readonly IPermissionRepository _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));

    /// <summary>
    /// 根据Id获取角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<RoleDetailViewModel> GetAsync(Guid id)
    {
        var role = await _cacheManager.GetAsync(
                       GirvsEntityCacheDefaults<Role>.ByIdCacheKey.Create(id.ToString()),
                       () => _roleRepository.GetByIdAsync(id))
                ?? throw new GirvsException("未找到对应的用户", StatusCodes.Status404NotFound);
        return role.MapToDto<RoleDetailViewModel>();
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post)]
    public async Task<RoleEditViewModel> CreateAsync([FromBody] RoleEditViewModel model)
    {
        var command = new CreateRoleCommand(model.Name, model.Desc, model.UserIds);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit)]
    public async Task<RoleEditViewModel> UpdateAsync(Guid id, [FromBody] RoleEditViewModel model)
    {
        var command = new UpdateRoleCommand(model.Id.Value, model.Name, model.Desc, model.UserIds);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var command = new DeleteRoleCommand(id);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<RoleListViewModel>> GetAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles?.MapTo<List<RoleListViewModel>>();
    }

    /// <summary>
    /// 添加角色用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="model"></param>
    /// <exception cref="GirvsException"></exception>
    [HttpPost("{roleId:guid}/User")]
    [ServiceMethodPermissionDescriptor("用户管理", Permission.Post_Extend1)]
    public async Task AddRoleUser(Guid roleId, [FromBody] EditRoleUserViewModel model)
    {
        var command = new AddRoleUserCommand(roleId, model.UserIds);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 删除角色用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="model"></param>
    /// <exception cref="GirvsException"></exception>
    [HttpDelete("{roleId:guid}/User")]
    [ServiceMethodPermissionDescriptor("用户管理", Permission.Post_Extend1)]
    public async Task DeleteRoleUser(Guid roleId, [FromBody] EditRoleUserViewModel model)
    {
        var command = new DeleteRoleUserCommand(roleId, model.UserIds);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取角色下指定的用户集合
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpGet("{roleId:guid}")]
    [ServiceMethodPermissionDescriptor("用户管理", Permission.Post_Extend1)]
    public async Task<List<UserQueryListViewModel>> GetRoleUsers(Guid roleId)
    {
        var role = await _roleRepository.GetRoleByIdIncludeUsersAsync(roleId)
                ?? throw new GirvsException(StatusCodes.Status404NotFound, "未找到相应数据");

        return role.Users.MapTo<List<UserQueryListViewModel>>();
    }

    /// <summary>
    /// 获取指定角色的功能菜单操作权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpGet("{roleId:guid}")]
    [ServiceMethodPermissionDescriptor("权限管理", Permission.Read)]
    public async Task<List<AuthorizePermissionModel>> GetRolePermission(Guid roleId)
    {
        var roleBasalPermissions = await _permissionRepository
            .GetWhereAsync(x => x.AppliedObjectType == PermissionAppliedObjectType.Role
                             && x.ValidateObjectType == PermissionValidateObjectType.FunctionMenu
                             && x.AppliedId == roleId);
        var mergeBasalPermissions = PermissionHelper.MergeValidateObjectTypePermission(roleBasalPermissions);
        var allAuthorizePermissions = (await _authorizationService.GetFunctionOperateList()).ToList();
        var dicAllAuthorizePermissions = allAuthorizePermissions.ToDictionary(item => item.ServiceId);
        return mergeBasalPermissions.Where(x => dicAllAuthorizePermissions.ContainsKey(x.AppliedObjectId))
                                    .Select(p => new AuthorizePermissionModel(
                                                string.Empty,
                                                p.AppliedObjectId,
                                                dicAllAuthorizePermissions[p.AppliedObjectId].Tag,
                                                0,
                                                dicAllAuthorizePermissions[p.AppliedObjectId].SystemModule,
                                                null,
                                                null,
                                                PermissionHelper.ConvertPermissionToString(p).ToDictionary(x => x, x => x)
                                            )).ToList();
    }

    /// <summary>
    /// 保存指定角色的权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost("{roleId:guid}")]
    [ServiceMethodPermissionDescriptor("权限管理", Permission.Read)]
    public async Task SaveRolePermission(Guid roleId, [FromBody] List<AuthorizePermissionModel> models)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);

        if (role == null)
        {
            throw new GirvsException("未找到对应的角色", StatusCodes.Status404NotFound);
        }

        var command = new SavePermissionCommand(
            roleId,
            PermissionAppliedObjectType.Role,
            PermissionValidateObjectType.FunctionMenu,
            models.Select(x => new ObjectPermission
            {
                AppliedObjectId = x.ServiceId,
                PermissionOperation =
                    PermissionHelper.ConvertStringToPermission(x.Permissions.Select(pair => pair.Value).ToList())
            }).ToList());

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}