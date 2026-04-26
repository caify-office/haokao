using Girvs.BusinessBasis;

namespace HaoKao.DeepSeekService.Domain;

public interface IDeepSeekConfigRepository : IManager
{
    Task<DeepSeekConfig> GetByTenantId(Guid tenantId);

    Task SaveAsync(DeepSeekConfig entity);
}