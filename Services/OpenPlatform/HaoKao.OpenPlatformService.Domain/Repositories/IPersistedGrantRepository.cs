using Girvs.BusinessBasis;

namespace HaoKao.OpenPlatformService.Domain.Repositories;

public interface IPersistedGrantRepository : IManager
{
    Task<IReadOnlyList<PersistedGrant>> GetBySubjectIdAndClientId(string subjectId, string clientId);

    Task<IReadOnlyList<PersistedGrant>> GetByClientIdAndSessionId(string clientId, string sessionId);

    Task<IReadOnlyList<PersistedGrant>> GetBySessionId(string sessionId);

    Task AddAsync(PersistedGrant entity);

    Task DeleteAsync(string subjectId, string clientId, string sessionId);

    Task ClearExpiredAsync();

    Task ConsumeAsync(string subjectId, string clientId, string sessionId);

    Task<bool> IsConsumedAsync(string subjectId, string clientId, string sessionId);
}