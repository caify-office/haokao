using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Queries;

namespace HaoKao.SalespersonService.Domain.Repositories;

public interface ISalespersonRepository : IRepository<Salesperson>
{
    Task<Salesperson> GetWithConfigById(Guid id);

    Task<SalespersonQuery> GetWithConfigByQuery(SalespersonQuery query);

    Task<IReadOnlyList<Salesperson>> GetIncludeAll();

    Task DeleteByIds(IReadOnlyList<Guid> ids);
}