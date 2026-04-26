using System.Linq;

namespace HaoKao.OpenPlatformService.Infrastructure.Repositories;

public class DomainProxyRepository : Repository<DomainProxy>, IDomainProxyRepository
{
    public Task<List<DomainProxy>> GetCurrentTenantDomainList()
    {
        var currentTenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>();
        return Queryable.Include(x => x.AccessClient).Where(x => x.TenantId == currentTenantId).ToListAsync();
    }

    public Task<DomainProxy> GetByDomain(string domian)
    {
        return Queryable.Include(x => x.AccessClient).FirstOrDefaultAsync(x => x.Domain == domian);
    }
}