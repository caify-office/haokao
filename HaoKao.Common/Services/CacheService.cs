namespace HaoKao.Common.Services;

[DynamicWebApi]
public class CacheService(IStaticCacheManager cacheManager, IRedisCacheService redisCacheService, ITypeFinder typeFinder) : ICacheService
{
    /// <summary>
    /// 获取所有的键Key
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<string>> GetKeys()
    {
        return await redisCacheService.RedisGetCacheKeys();
    }

    /// <summary>
    /// 根据Key获取缓存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("{key}")]
    public async Task<string> GetKeyValue(string key)
    {
        var value = await cacheManager.GetAsync(new CacheKey(key).Create(), () => Task.FromResult<dynamic>(null!));
        return JsonConvert.SerializeObject(value ?? string.Empty);
    }

    /// <summary>
    /// 根据Key删除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpDelete("{key}")]
    public async Task DeleteByKey(string key)
    {
        await cacheManager.RemoveAsync(new CacheKey(key).Create());
    }

    /// <summary>
    /// 获取所有的实体缓存
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<string>> GetEntityKeys()
    {
        var entityTypes = typeFinder.FindOfType<Entity>();
        var result = entityTypes.Select(t => $"{t.FullName},{t.Assembly.GetName().Name}").ToList();
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 根据实体清除列表缓存
    /// </summary>
    /// <param name="entityFullName"></param>
    /// <returns></returns>
    [HttpDelete("{entityFullName}")]
    public Task DeleteList(string entityFullName)
    {
        RemoveCache(entityFullName, nameof(GirvsEntityCacheDefaults<Entity>.ListCacheKey));
        return Task.CompletedTask;
    }

    /// <summary>
    /// 根据实体清除与当前实体相关的缓存
    /// </summary>
    /// <param name="entityFullName"></param>
    /// <returns></returns>
    [HttpDelete("{entityFullName}")]
    public Task DeleteEntity(string entityFullName)
    {
        RemoveCache(entityFullName, nameof(GirvsEntityCacheDefaults<Entity>.ByTenantKey));
        return Task.CompletedTask;
    }

    private void RemoveCache(string entityFullName, string entityCacheDefaultPropertyName)
    {
        var entityType = Type.GetType(entityFullName);
        var cacheKeyManagerType = typeof(GirvsEntityCacheDefaults<>);
        if (entityType == null || cacheKeyManagerType == null)
        {
            throw new GirvsException("未知的缓存数据类型");
        }

        var property = cacheKeyManagerType.MakeGenericType([entityType,])
                                          .GetProperty(entityCacheDefaultPropertyName);
        if (property == null)
        {
            throw new GirvsException("未知的缓存数据类型");
        }

        if (property.GetValue(cacheKeyManagerType) is not CacheKey entityListCacheKey)
        {
            throw new GirvsException("未知的缓存数据类型");
        }

        cacheManager.RemoveByPrefix(entityListCacheKey.Key);
    }
}