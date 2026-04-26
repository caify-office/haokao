namespace HaoKao.OpenPlatformService.Domain.Repositories;

public interface IDomainProxyRepository : IRepository<DomainProxy>
{
    Task<List<DomainProxy>> GetCurrentTenantDomainList();

    Task<DomainProxy> GetByDomain(string domian);
}