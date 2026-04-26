using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories.Base;

namespace ShortUrlService.Domain.Repositories;

public interface IAccessLogRepository : IRepository<AccessLog, long>, IManager
{
    /// <summary>
    /// 获取短链接的访问日志
    /// </summary>
    /// <param name="shortUrlId"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(int TotalCount, IReadOnlyList<AccessLog>)> GetPagedListAsync(long shortUrlId, int pageIndex, int pageSize);

    /// <summary>
    /// 查询时间段内的的访问记录
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<IReadOnlyList<AccessLog>> GetListAsync(DateTime start, DateTime end);

    /// <summary>
    /// 查询短链接的访问次数
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<int> CountAsync(DateTime start, DateTime end);

    /// <summary>
    /// 查询不同浏览器下短链接的访问次数
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<Dictionary<int, int>> GetCountGroupByBrowserAsync(DateTime start, DateTime end);

    /// <summary>
    /// 查询不同操作系统下短链接的访问次数
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<Dictionary<int, int>> GetCountGroupByOsAsync(DateTime start, DateTime end);
}