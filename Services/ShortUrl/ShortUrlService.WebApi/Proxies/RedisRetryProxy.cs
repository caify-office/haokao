using Polly;
using ShortUrlService.WebApi.Configurations;
using StackExchange.Redis;

namespace ShortUrlService.WebApi.Proxies;

public class RedisRetryProxy
{
    private readonly IDatabase _redisDb;
    private readonly Polly.Retry.AsyncRetryPolicy _retryPolicy;

    public RedisRetryProxy(IDatabase redisDb, IConfiguration configuration)
    {
        _redisDb = redisDb ?? throw new ArgumentNullException(nameof(redisDb));
        var durations = Singleton<AppSettings>.Instance.Get<RedisRetryPolicyConfig>().SleepDurations.Select(x => TimeSpan.FromSeconds(x)).ToArray();
        _retryPolicy = Policy.Handle<RedisTimeoutException>().WaitAndRetryAsync(durations);
    }

    public Task<RedisValue> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
    {
        return _retryPolicy.ExecuteAsync(() => _redisDb.StringGetAsync(key, flags));
    }

    public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
    {
        return _retryPolicy.ExecuteAsync(() => _redisDb.StringSetAsync(key, value, expiry, when, flags));
    }

    public Task<long> StringIncrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
    {
        return _retryPolicy.ExecuteAsync(() => _redisDb.StringIncrementAsync(key, value, flags));
    }
}