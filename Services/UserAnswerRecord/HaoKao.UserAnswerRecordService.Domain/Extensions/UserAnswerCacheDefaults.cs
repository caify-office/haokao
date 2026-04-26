using Girvs.Infrastructure;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Extensions;

public class UserAnswerCacheDefaults
{
    /// <summary>
    /// 获取缓存键中使用的实体类型名称
    /// </summary>
    private string EntityTypeName => nameof(UserAnswerRecord).ToLowerInvariant();

    private static string UserKey
    {
        get
        {
            var property = typeof(UserAnswerRecord).GetProperty(nameof(IIncludeCreatorId<Guid>.CreatorId));
            var userId = EngineContext.Current.IsAuthenticated
                ? EngineContext.Current.ClaimManager.GetUserId()
                : Guid.Empty.ToString();
            return property == null
                ? string.Empty
                : $":CreatorId_{userId}";
        }
    }

    private static string TenantKey
    {
        get
        {
            var property = typeof(UserAnswerRecord).GetProperty("TenantId");
            var tenantId = EngineContext.Current.IsAuthenticated
                ? EngineContext.Current.ClaimManager.GetTenantId()
                : EngineContext.Current.HttpContext.Request.Headers["TenantId"].To<Guid>().ToString();
            return property == null
                ? string.Empty
                : $":TenantId_{tenantId}";
        }
    }

    /// <summary>
    /// 统计刷新相关前缀
    /// </summary>
    public CacheKey RefreshPrefix => new($"{EntityTypeName}:refresh");

    /// <summary>
    /// 统计刷新相关缓存键
    /// </summary>
    public CacheKey Refresh(Guid userId) => new($"{EntityTypeName}:refresh{TenantKey}:CreatorId_{userId}:{{0}}");

    /// <summary>
    /// 统计结果相关缓存键
    /// </summary>
    public CacheKey Statistics(Guid userId) => new($"{EntityTypeName}:statistics{TenantKey}:CreatorId_{userId}:{{0}}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ById => new($"{EntityTypeName}{TenantKey}{UserKey}:byid:{{0}}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey Prefix => new($"{EntityTypeName}{TenantKey}{UserKey}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ListPrefix => new($"{EntityTypeName}{TenantKey}{UserKey}:List");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ListOther => new($"{EntityTypeName}{TenantKey}{UserKey}:List:Other:{{0}}");

    /// <summary>
    /// 通过租户标识符获取缓存实体的键
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public CacheKey ListQuery => new($"{EntityTypeName}{TenantKey}{UserKey}:List:Query:{{0}}");

    /// <summary>
    /// 创建自定义的缓存键
    /// </summary>
    /// <param name="key">自定义缓存键</param>
    /// <returns></returns>
    public CacheKey BuideCustomize(string key) => new($"{Prefix}:{key}");
}