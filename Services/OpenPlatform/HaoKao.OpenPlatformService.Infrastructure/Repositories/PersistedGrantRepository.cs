using System.Linq;
using Girvs.Cache.Caching;

namespace HaoKao.OpenPlatformService.Infrastructure.Repositories;

public class PersistedGrantRepository(OpenPlatformDbContext dbContext) : IPersistedGrantRepository
{
    public async Task<IReadOnlyList<PersistedGrant>> GetBySubjectIdAndClientId(string subjectId, string clientId)
    {
        var whitelist = await GetWhiteListAsync();
        if (whitelist.Contains(subjectId)) return [];

        return await dbContext.PersistedGrants.AsNoTracking()
                              .Where(x => x.SubjectId == subjectId && x.ClientId == clientId)
                              .OrderByDescending(x => x.CreationTime)
                              .ToListAsync();
    }

    public async Task<IReadOnlyList<PersistedGrant>> GetByClientIdAndSessionId(string clientId, string sessionId)
    {
        return await dbContext.PersistedGrants.AsNoTracking()
                              .Where(x => x.ClientId == clientId && x.SessionId == sessionId)
                              .OrderByDescending(x => x.CreationTime)
                              .ToListAsync();
    }

    public async Task<IReadOnlyList<PersistedGrant>> GetBySessionId(string sessionId)
    {
        return await dbContext.PersistedGrants.AsNoTracking()
                              .Where(x => x.SessionId == sessionId)
                              .OrderByDescending(x => x.CreationTime)
                              .ToListAsync();
    }

    private async Task<IReadOnlySet<string>> GetWhiteListAsync()
    {
        var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        var cacheTime = (int)TimeSpan.FromDays(365 * 10).TotalMinutes;
        var cacheKey = GirvsEntityCacheDefaults<PersistedGrant>.QueryCacheKey.Create("whitelist", cacheTime: cacheTime);
        return await cacheManager.GetAsync(cacheKey, () => Task.FromResult(new HashSet<string>()));
    }

    public async Task AddAsync(PersistedGrant entity)
    {
        // ensure key is unique
        var existing = await dbContext.PersistedGrants.FirstOrDefaultAsync(x => x.Key == entity.Key);
        if (existing != null) return;

        entity.Id = Guid.NewGuid();
        await dbContext.PersistedGrants.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(string subjectId, string clientId, string sessionId)
    {
        var grant = await dbContext.PersistedGrants.FirstOrDefaultAsync(x => x.SubjectId == subjectId
                                                                          && x.ClientId == clientId
                                                                          && x.SessionId == sessionId);
        if (grant == null) return;
        dbContext.PersistedGrants.Remove(grant);
        await dbContext.SaveChangesAsync();
    }

    public async Task ClearExpiredAsync()
    {
        var expiredGrants = await dbContext.PersistedGrants.Where(x => x.Expiration < DateTime.Now).ToListAsync();
        if (expiredGrants.Count == 0) return;
        dbContext.PersistedGrants.RemoveRange(expiredGrants);
    }

    public Task ConsumeAsync(string subjectId, string clientId, string sessionId)
    {
        var grant = dbContext.PersistedGrants.FirstOrDefault(x => x.SubjectId == subjectId
                                                               && x.ClientId == clientId
                                                               && x.SessionId == sessionId);
        if (grant == null) return Task.CompletedTask;
        grant.ConsumedTime = DateTime.Now;
        return dbContext.SaveChangesAsync();
    }

    public Task<bool> IsConsumedAsync(string subjectId, string clientId, string sessionId)
    {
        return dbContext.PersistedGrants.AnyAsync(x => x.SubjectId == subjectId
                                                    && x.ClientId == clientId
                                                    && x.SessionId == sessionId
                                                    && x.ConsumedTime != null);
    }
}