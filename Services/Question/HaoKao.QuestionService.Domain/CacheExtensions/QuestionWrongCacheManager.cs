using Girvs.Infrastructure;
using HaoKao.QuestionService.Domain.QuestionWrongModule;

namespace HaoKao.QuestionService.Domain.CacheExtensions;

public static class QuestionWrongCacheManager
{
    private static Type EntityType => typeof(QuestionWrong);

    private static string EntityTypeName => nameof(QuestionWrong).ToLowerInvariant();

    private static string UserKey
    {
        get
        {
            var property = EntityType.GetProperty(nameof(IIncludeCreatorId<Guid>.CreatorId));
            return property == null
                ? string.Empty
                : $":CreatorId_{EngineContext.Current.ClaimManager.IdentityClaim.UserId}";
        }
    }

    private static string TenantKey
    {
        get
        {
            var property = EntityType.GetProperty("TenantId");
            return property == null
                ? string.Empty
                : $":TenantId_{EngineContext.Current.ClaimManager.IdentityClaim.TenantId}";
        }
    }

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey ById => new($"{EntityTypeName}{TenantKey}{UserKey}:byid:{{0}}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey Prefix => new($"{EntityTypeName}{TenantKey}{UserKey}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey ListPrefix => new($"{EntityTypeName}{TenantKey}{UserKey}:List");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey ListOther => new($"{EntityTypeName}{TenantKey}{UserKey}:List:Other:{{0}}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey ListQuery => new($"{EntityTypeName}{TenantKey}{UserKey}:List:Query:{{0}}");

    /// <summary>
    /// 创建自定义的缓存键
    /// </summary>
    /// <param name="key">自定义缓存键</param>
    /// <returns></returns>
    public static CacheKey BuideCustomize(string key) => new($"{Prefix}:{key}");
}