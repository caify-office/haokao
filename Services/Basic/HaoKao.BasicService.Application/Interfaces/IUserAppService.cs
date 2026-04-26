namespace HaoKao.BasicService.Application.Interfaces;

public interface IUserAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据Id获取用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserDetailViewModel> GetAsync(Guid id);

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserEditViewModel> CreateAsync(UserEditViewModel model);

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserEditViewModel> UpdateAsync(Guid id, UserEditViewModel model);

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// 根据查询条件获取用户
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserQueryViewModel> GetAsync(UserQueryViewModel model);

    /// <summary>
    /// 添加用户角色操作
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddUserRole(Guid userId, EditUserRoleViewModel model);

    /// <summary>
    /// 删除用户角色操作
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task DeleteUserRole(Guid userId, EditUserRoleViewModel model);

    /// <summary>
    /// 启用用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task Enable(Guid userId);

    /// <summary>
    /// 禁用用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task Disable(Guid userId);

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task ChangeUserPassword(Guid userId, ChangeUserPassworkViewModel model);

    /// <summary>
    /// 用户自行修改密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task UserEditPassword(UserEditPasswordViewModel model);

    /// <summary>
    /// 获取用户所包含的角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<RoleListViewModel>> GetUserRoles(Guid userId);

    /// <summary>
    /// 判断登陆名称是否已存在
    /// </summary>
    /// <param name="account"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<bool> ExistAccount(string account, Guid tenantId);

    /// <summary>
    /// 获取当前用户的权限、包含功能菜单以及数据权限
    /// </summary>
    /// <returns></returns>
    Task<AuthorizeModel> GetCurrentUserAuthorization();

    /// <summary>
    /// 获取指定用户的权限
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<AuthorizeModel> GetCurrentUserAuthorization(Guid userId);

    /// <summary>
    /// 获取指定用户的数据权限
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<UserDataRuleListViewModel>> GetUserDataRule(Guid userId);

    /// <summary>
    /// 保存指定用户的数据权限
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="models"></param>
    /// <returns></returns>
    Task SaveUserDataRule(Guid userId, List<SaveUserDataRuleViewModel> models);
}