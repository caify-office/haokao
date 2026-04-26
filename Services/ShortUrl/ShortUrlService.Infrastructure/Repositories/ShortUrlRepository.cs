using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;
using ShortUrlService.Domain.Specifications;
using ShortUrlService.Infrastructure.Extensions;
using ShortUrlService.Infrastructure.Repositories.Base;

namespace ShortUrlService.Infrastructure.Repositories;

public class ShortUrlRepository(ShortUrlDbContext context) : Repository<ShortUrl, long>(context), IShortUrlRepository
{
    public Task<ShortUrl?> GetByShortKey(string shortKey)
    {
        var specification = new ShortUrlByShortKeySpec(shortKey);
        return GetAsync(specification);
    }

    public Task<int> GetAccessCountAsync(long id)
    {
        var specification = new ShortUrlWithAccessLogSpec(id);
        return context.ShortUrls.SatisfiedBy(specification).Select(x => x.AccessLogs.Count).FirstOrDefaultAsync();
    }

    public Task<ShortUrl?> GetForRegisterApp(long registerAppId, string originUrl)
    {
        var specification = new ShortUrlSpecification(registerAppId, originUrl);
        return GetAsync(specification);
    }

    public async Task<(int TotalCount, IReadOnlyList<ShortUrl>)> GetPagedListAsync(long? registerAppId, int pageIndex, int pageSize)
    {
        var query = context.ShortUrls.Where(x => !x.IsDelete);
        if (registerAppId.HasValue)
        {
            query = query.Where(x => x.RegisterAppId == registerAppId);
        }

        var count = await query.CountAsync();
        return count == 0
            ? (count, [])
            : (count, await query.OrderByDescending(x => x.CreateTime)
                                 .Skip((pageIndex - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync());
    }

    public Task<Dictionary<long, int>> GetAccessCountAsync(IEnumerable<long> ids)
    {
        var specification = new ShortUrlWithAccessLogSpec(ids);
        return context.ShortUrls.SatisfiedBy(specification)
                      .Select(x => new { x.Id, x.AccessLogs.Count })
                      .ToDictionaryAsync(x => x.Id, x => x.Count);
    }

    public Task<int> CountAsync(DateTime start, DateTime end)
    {
        return context.ShortUrls.CountAsync(x => x.CreateTime >= start && x.CreateTime < end);
    }

    public async Task<IReadOnlyList<ShortUrl>> GetListAsync(DateTime start, DateTime end)
    {
        return await context.ShortUrls.Where(x => x.CreateTime >= start && x.CreateTime < end)
                            .OrderByDescending(x => x.CreateTime)
                            .ToListAsync();
    }
}