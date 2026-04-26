using HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

public interface IRegisterUserService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseRegisterUserViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<RegisterUserQueryViewModel> Get(RegisterUserQueryViewModel queryViewModel);

    /// <summary>
    /// 根据主键删除指定注册用户
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 禁用用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Disable(Guid id);

    /// <summary>
    /// 启用用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Enable(Guid id);

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> ResetPassword(Guid id, string password);

    /// <summary>
    /// 修改用户手机号码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    Task<bool> ModifyPhone(Guid id, string phone);
}