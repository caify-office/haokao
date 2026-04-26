using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories.Base;

namespace ShortUrlService.Domain.Repositories;

public interface IRegisterAppRepository : IRepository<RegisterApp, long>, IManager
{
    /// <summary>
    /// 新增注册应用时查询是否已存在
    /// </summary>
    /// <param name="appName"></param>
    /// <param name="appCode"></param>
    /// <returns></returns>
    Task<bool> ExistForCreate(string appName, string appCode);

    /// <summary>
    /// 根据应用编码和密钥查询应用
    /// </summary>
    /// <param name="appCode"></param>
    /// <param name="appSecret"></param>
    /// <returns></returns>
    Task<RegisterApp?> GetByCodeAndSecret(string appCode, string appSecret);
}