using HaoKao.TenantService.Domain.Entities;

namespace HaoKao.TenantService.Domain.Repositories;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<Tenant> GetByNameAsync(string name);

    Task<Tenant> GetByNoAsync(string no);
}