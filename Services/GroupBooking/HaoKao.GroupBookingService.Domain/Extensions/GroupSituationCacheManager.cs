using Girvs.Infrastructure;
using HaoKao.GroupBookingService.Domain.Entities;
using System;

namespace HaoKao.GroupBookingService.Domain.Extensions;

public static class GroupSituationCacheManager
{
    /// <summary>
    /// 获取缓存键中使用的实体类型名称
    /// </summary>
    private static string EntityTypeName => nameof(GroupData).ToLowerInvariant();

    private static string UserKey
    {
        get
        {
            var property = typeof(GroupData).GetProperty(nameof(IIncludeCreatorId<Guid>.CreatorId));
            return property == null
                ? string.Empty
                : $":CreatorId{EngineContext.Current.ClaimManager.IdentityClaim.UserId}";
        }
    }

    private static string TenantKey
    {
        get
        {
            var property = typeof(GroupData).GetProperty("TenantId");
            return property == null
                ? string.Empty
                : $":TenantId_{EngineContext.Current.ClaimManager.IdentityClaim.TenantId}";
        }
    }

    public static CacheKey MySuccessGroupSituationCacheKey => new CacheKey($"{EntityTypeName}{TenantKey}:byuserid:{{0}}:MySuccessGroupSituation");
}