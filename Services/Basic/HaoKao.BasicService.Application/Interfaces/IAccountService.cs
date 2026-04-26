namespace HaoKao.BasicService.Application.Interfaces;

public interface IAccountService : IAppWebApiService
{
    /// <summary>
    /// 通过登陆名称，获取相关的考试列表
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task<List<TenantEntityViewModel>> GetTenants(string account);

    /// <summary>
    /// 登陆第一步验证，用户名和密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserLoginStepOneReturnViewModel> UserLoginStepOne(UserLoginStepOneViewModel model);

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> SendPhoneCode(SendPhoneCodeViewModel model);

    /// <summary>
    /// 登陆第二步验证，用户名和密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserLoginDetailViewModel> UserLoginStepSecond(UserLoginSecondStepViewModel model);

    /// <summary>
    /// 考试管理员登录进去后输入目标考试管理员账号和密码信息获取对应考试新的token
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserLoginDetailViewModel> ChangeTenant([FromBody] ChangeExamViewModel model);

    /// <summary>
    /// 更换绑定手机号码发送验证码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> SendChangePhoneCode(SendChangePhoneCodeViewModel model);

    /// <summary>
    /// 更换手机号码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> ChangePhoneCode(ChangePhoneCodeViewModel model);
}