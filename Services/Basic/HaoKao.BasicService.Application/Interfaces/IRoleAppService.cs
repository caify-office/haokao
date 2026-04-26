namespace HaoKao.BasicService.Application.Interfaces;

public interface IRoleAppService : IAppWebApiService
{
    /// <summary>
    /// 根据Id获取角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RoleDetailViewModel> GetAsync(Guid id);

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<RoleEditViewModel> CreateAsync(RoleEditViewModel model);

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<RoleEditViewModel> UpdateAsync(Guid id, RoleEditViewModel model);

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <returns></returns>
    Task<List<RoleListViewModel>> GetAsync();

    /// <summary>
    /// 添加角色用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddRoleUser(Guid roleId, EditRoleUserViewModel model);

    /// <summary>
    /// 删除角色用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task DeleteRoleUser(Guid roleId, EditRoleUserViewModel model);

    /// <summary>
    /// 获取角色下指定的用户集合
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<List<UserQueryListViewModel>> GetRoleUsers(Guid roleId);

    /// <summary>
    /// 获取指定角色的权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<List<AuthorizePermissionModel>> GetRolePermission(Guid roleId);

    /// <summary>
    /// 保存指定角色的权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="models"></param>
    /// <returns></returns>
    Task SaveRolePermission(Guid roleId, List<AuthorizePermissionModel> models);
}