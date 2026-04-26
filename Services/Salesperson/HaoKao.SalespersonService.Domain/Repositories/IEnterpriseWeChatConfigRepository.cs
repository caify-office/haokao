using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Domain.Repositories;

public interface IEnterpriseWeChatConfigRepository : IRepository<EnterpriseWeChatConfig>
{
    Task DeleteByIds(IReadOnlyList<Guid> ids);
}