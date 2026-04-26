using Girvs.Infrastructure;

namespace HaoKao.QuestionService.Domain.CacheExtensions;

public class GenericCacheManager(Type t)
{
    private readonly string _entityTypeName = t.Name.ToLowerInvariant();

    private string UserKey
    {
        get
        {
            var property = t.GetProperty(nameof(IIncludeCreatorId<Guid>.CreatorId));
            return property == null
                ? string.Empty
                : $":CreatorId{EngineContext.Current.ClaimManager.GetUserId()}";
        }
    }

    private string TenantKey
    {
        get
        {
            var property = t.GetProperty("TenantId");
            return property == null
                ? string.Empty
                : $":TenantId_{EngineContext.Current.ClaimManager.GetTenantId()}";
        }
    }

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ById => new($"{_entityTypeName}{TenantKey}{UserKey}:byid:{{0}}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey Prefix => new($"{_entityTypeName}{TenantKey}{UserKey}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ListPrefix => new($"{_entityTypeName}{TenantKey}{UserKey}:List");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ListOther => new($"{_entityTypeName}{TenantKey}{UserKey}:List:Other:{{0}}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ListQuery => new($"{_entityTypeName}{TenantKey}{UserKey}:List:Query:{{0}}");

    /// <summary>
    /// 创建自定义的缓存键
    /// </summary>
    /// <param name="key">自定义缓存键</param>
    /// <returns></returns>
    public CacheKey BuideCustomize(string key) => new($"{Prefix}:{key}");
}