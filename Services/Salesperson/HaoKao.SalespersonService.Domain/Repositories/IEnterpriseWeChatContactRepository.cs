using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Domain.Repositories;

public interface IEnterpriseWeChatContactRepository : IRepository<EnterpriseWeChatContact>
{
    Task<IReadOnlyList<EnterpriseWeChatContact>> GetByFollowUserId(string followUserId);
}