using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;
using ShortUrlService.Infrastructure.Repositories.Base;

namespace ShortUrlService.Infrastructure.Repositories;

public class AccessLogRepository(ShortUrlDbContext dbContext) : Repository<AccessLog, long>(dbContext), IAccessLogRepository
{
    public async Task<(int TotalCount, IReadOnlyList<AccessLog>)> GetPagedListAsync(long shortUrlId, int pageIndex, int pageSize)
    {
        var count = dbContext.AccessLogs.Count(x => x.ShortUrlId == shortUrlId);
        return count == 0
            ? (count, [])
            : (count, await dbContext.AccessLogs
                                     .Where(x => x.ShortUrlId == shortUrlId)
                                     .OrderByDescending(x => x.CreateTime)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync());
    }

    public Task<int> CountAsync(DateTime start, DateTime end)
    {
        return dbContext.AccessLogs.CountAsync(x => x.CreateTime >= start && x.CreateTime < end);
    }

    public async Task<IReadOnlyList<AccessLog>> GetListAsync(DateTime start, DateTime end)
    {
        return await dbContext.AccessLogs
                              .Where(x => x.CreateTime >= start && x.CreateTime < end)
                              .OrderByDescending(x => x.CreateTime)
                              .ToListAsync();
    }

    public Task<Dictionary<int, int>> GetCountGroupByBrowserAsync(DateTime start, DateTime end)
    {
        return dbContext.AccessLogs
                        .Where(x => x.CreateTime >= start && x.CreateTime < end)
                        .GroupBy(x => x.BrowserType)
                        .Select(x => new { x.Key, Count = x.Count() })
                        .ToDictionaryAsync(x => x.Key, x => x.Count);
    }

    public Task<Dictionary<int, int>> GetCountGroupByOsAsync(DateTime start, DateTime end)
    {
        return dbContext.AccessLogs
                        .Where(x => x.CreateTime >= start && x.CreateTime < end)
                        .GroupBy(x => x.OsType)
                        .Select(x => new { x.Key, Count = x.Count() })
                        .ToDictionaryAsync(x => x.Key, x => x.Count);
    }
}