using System;
using System.Collections.Generic;

namespace HaoKao.GroupBookingService.Domain.Entities;

/// <summary>
/// 拼团资料
/// </summary>
public class GroupData : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 资料名称
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
    /// 基础拼团成功人数
    /// </summary>
    public int BasePeopleNumber { get; set; }

    /// <summary>
    /// 限制时间
    /// </summary>
    public int LimitTime { get; set; }

    /// <summary>
    /// 拼团资料
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool? State { get; set; }

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
