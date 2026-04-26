using StackExchange.Redis;

namespace HaoKao.Common.Services;

public interface IRedisCacheService : IManager
{
    Task<HashEntry[]> RedisHashGetAllAsync(string key);

    Task RedisHashDeleteAsync(string key, string name);

    Task RedisHashSetAsync(string key, HashEntry[] hashEntries);

    Task<IList<string>> RedisGetCacheKeys(string prefix = null, bool hasInstanceName = false);

    Task<long> RedisStringIncrementAsync(string key);

    Task RedisListSetAsync(string key, dynamic value);

    Task<List<T>> RedisListGetAsync<T>(string key, long count);

    Task<T> RedisListGetAsync<T>(string key);

    Task RedisStreamSetAsync(string taskStream, dynamic value);

    Task RedisStreamCreateConsumerGroupAsync(string taskStream, string taskGroup);

    Task<StreamEntry[]> RedisStreamGetAsync(string taskStream, string taskGroup, int count);

    Task RedisStreamAcknowledgeAsync(string taskStream, string taskGroup, string id);
}