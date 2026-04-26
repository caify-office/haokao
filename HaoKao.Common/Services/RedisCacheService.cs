using Girvs.Cache.CacheImps;
using Girvs.Cache.Configuration;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HaoKao.Common.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;
    private readonly string _instanceName;
    private readonly IRedisConnectionWrapper _connectionWrapper;

    public RedisCacheService(IOptions<RedisCacheOptions> optionsAccessor)
    {
        var config = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        _connectionWrapper = new RedisConnectionWrapper(optionsAccessor.Value);
        _db = _connectionWrapper.GetDatabase();
        _instanceName = config.DistributedCacheConfig.InstanceName;
    }

    public async Task<HashEntry[]> RedisHashGetAllAsync(string key)
    {
        return await _db.HashGetAllAsync($"{_instanceName}{key}");
    }

    public async Task RedisHashDeleteAsync(string key, string name)
    {
        await _db.HashDeleteAsync($"{_instanceName}{key}", name);
    }

    public async Task RedisHashSetAsync(string key, HashEntry[] hashEntries)
    {
        await _db.HashSetAsync($"{_instanceName}{key}", hashEntries);
    }

    public async Task<long> RedisStringIncrementAsync(string key)
    {
        return await _db.StringIncrementAsync($"{_instanceName}{key}");
    }

    public async Task<IList<string>> RedisGetCacheKeys(string prefix = null, bool hasInstanceName = false)
    {
        var redisKeys = new List<string>();
        foreach (var endPoint in await _connectionWrapper.GetEndPointsAsync())
        {
            var server = await _connectionWrapper.GetServerAsync(endPoint);
            var keys = server.Keys(_db.Database, string.IsNullOrEmpty(prefix) ? null : $"{_instanceName}{prefix}*")
                             .ToArray();
            redisKeys.AddRange(keys.Select(c => c.ToString()));
        }
        redisKeys = redisKeys.Select(c => hasInstanceName || !c.StartsWith(_instanceName) ? c : c[_instanceName.Length..]).ToList();
        return redisKeys;
    }

    public async Task RedisListSetAsync(string key, dynamic value)
    {
        string valueStr = JsonConvert.SerializeObject(value);
        await _db.ListRightPushAsync($"{_instanceName}{key}", valueStr);
    }

    public async Task<List<T>> RedisListGetAsync<T>(string key, long count)
    {
        var taskJsonList = await _db.ListLeftPopAsync($"{_instanceName}{key}", count);
        return taskJsonList == null ? default : JsonConvert.DeserializeObject<List<T>>($"[{string.Join(",", taskJsonList)}]");
    }

    public async Task<T> RedisListGetAsync<T>(string key)
    {
        var taskJsonList = await _db.ListLeftPopAsync($"{_instanceName}{key}");
        return !taskJsonList.HasValue ? default : JsonConvert.DeserializeObject<T>($"[{string.Join(",", taskJsonList)}]");
    }

    public async Task RedisStreamSetAsync(string taskStream, dynamic value)
    {
        string taskJson = JsonConvert.SerializeObject(value);
        await _db.StreamAddAsync(taskStream, [new NameValueEntry("payload", taskJson)]);
    }

    public async Task RedisStreamCreateConsumerGroupAsync(string taskStream, string taskGroup)
    {
        await _db.StreamCreateConsumerGroupAsync(taskStream, taskGroup, "0", true);
    }

    public async Task<StreamEntry[]> RedisStreamGetAsync(string taskStream, string taskGroup, int count)
    {
        return await _db.StreamReadGroupAsync(taskStream, taskGroup, "consumer1", ">", count: count);
    }

    public async Task RedisStreamAcknowledgeAsync(string taskStream, string taskGroup, string id)
    {
        await _db.StreamAcknowledgeAsync(taskStream, taskGroup, id);
    }
}