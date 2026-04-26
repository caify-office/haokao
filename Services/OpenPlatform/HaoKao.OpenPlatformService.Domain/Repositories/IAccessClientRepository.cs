namespace HaoKao.OpenPlatformService.Domain.Repositories;

public interface IAccessClientRepository : IRepository<AccessClient>
{
    Task<AccessClient> GetByClientId(string clientId);

    Task<AccessClient> GetById(Guid id);
}