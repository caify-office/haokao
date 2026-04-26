using System;
using System.Collections.Generic;

namespace HaoKao.GroupBookingService.Domain.Entities;

/// <summary>
/// 拼团情况
/// </summary>
public class GroupSituation : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    public GroupSituation()
    {
        GroupMembers = [];
    }

    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid GroupDataId { get; set; }

    /// <summary>
    /// 组团资料名称
    /// </summary>
    public string DataName { get; set; }

    /// <summary>
    /// 适用科目
    /// </summary>
    public List<Guid> SuitableSubjects { get; set; }

    /// <summary>
    /// 成团人数
    /// </summary>
    public int PeopleNumber { get; set; }

    /// <summary>
    /// 限制时间
    /// </summary>
    public int LimitTime { get; set; }

    /// <summary>
    /// 拼团资料
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// 组团成员
    /// </summary>
    public virtual List<GroupMember> GroupMembers { get; set; }

    /// <summary>
    /// 拼团成功时间
    /// </summary>
    public DateTime? SuccessTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}

