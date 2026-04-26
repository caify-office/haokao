namespace HaoKao.BasicService.Application.Interfaces;

public interface IAuthorizationService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取需要授权的功能列表
    /// </summary>
    /// <returns></returns>
    Task<IList<AuthorizePermissionModel>> GetFunctionOperateList();

    /// <summary>
    /// 获取需要授权的数据规则列表
    /// </summary>
    /// <returns></returns>
    Task<IList<AuthorizeDataRuleModel>> GetDataRuleList();

    /// <summary>
    /// 初始化本模块的权限值
    /// </summary>
    /// <returns></returns>
    Task InitAuthorization();
}