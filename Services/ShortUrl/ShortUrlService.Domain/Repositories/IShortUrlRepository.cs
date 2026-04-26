using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories.Base;

namespace ShortUrlService.Domain.Repositories;

public interface IShortUrlRepository : IRepository<ShortUrl, long>, IManager
{
    /// <summary>
    /// 根据 shortKey 获取短链接
    /// </summary>
    /// <param name="shortKey"></param>
    /// <returns></returns>
    Task<ShortUrl?> GetByShortKey(string shortKey);

    /// <summary>
    /// 查询短链接的访问次数
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> GetAccessCountAsync(long id);

    /// <summary>
    /// 查询应用下的 OriginUrl 对应的短链接
    /// </summary>
    /// <param name="registerAppId"></param>
    /// <param name="originUrl"></param>
    /// <returns></returns>
    Task<ShortUrl?> GetForRegisterApp(long registerAppId, string originUrl);

    /// <summary>
    /// 获取应用下的短链接列表
    /// </summary>
    /// <param name="registerAppId"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(int TotalCount, IReadOnlyList<ShortUrl>)> GetPagedListAsync(long? registerAppId, int pageIndex, int pageSize);

    /// <summary>
    /// 查询短链接的访问次数
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<Dictionary<long, int>> GetAccessCountAsync(IEnumerable<long> ids);

    /// <summary>
    /// 查询短链接的生成次数
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<int> CountAsync(DateTime start, DateTime end);

    /// <summary>
    /// 查询短链接的生成记录
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ShortUrl>> GetListAsync(DateTime start, DateTime end);
}