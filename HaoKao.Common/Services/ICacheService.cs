namespace HaoKao.Common.Services;

public interface ICacheService : IAppWebApiService, IManager
{
    Task<IList<string>> GetKeys();

    Task<string> GetKeyValue(string key);

    Task DeleteByKey(string key);

    Task<IList<string>> GetEntityKeys();

    Task DeleteList(string entityFullName);

    Task DeleteEntity(string entityFullName);
}