using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Repositories;

namespace HaoKao.SalespersonService.Infrastructure.Repositories;

public class EnterpriseWeChatConfigRepository : Repository<EnterpriseWeChatConfig>, IEnterpriseWeChatConfigRepository
{
    public Task DeleteByIds(IReadOnlyList<Guid> ids)
    {
        return Queryable.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
    }
}