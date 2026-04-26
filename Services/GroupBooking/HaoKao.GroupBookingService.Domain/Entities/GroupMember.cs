using System;
using System.Text.Json.Serialization;

namespace HaoKao.GroupBookingService.Domain.Entities;

/// <summary>
/// 组团成员
/// </summary>
public class GroupMember : AggregateRoot<Guid>, IIncludeCreatorId<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 拼团情况Id
    /// </summary>
    public Guid GroupSituationId { get; set; }

    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid GroupDataId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 用户头像Url
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// 是否团长
    /// </summary>
    public bool IsLeader { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 拼团情况
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public GroupSituation GroupSituation { get; set; } = new();

    /// <summary>
    /// 成功时间
    /// </summary>
    public DateTime? SuccessTime;

    /// <summary>
    /// 过期时间
    /// </summary>

    public DateTime ExpirationTime { get; set; }
}
