using HaoKao.BurialPointService.Domain.Enums;
using System.Collections.Generic;

namespace HaoKao.BurialPointService.Domain.Entities;

/// <summary>
/// 埋点
/// </summary>
public class BurialPoint : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 所属端
    /// </summary>
    public BelongingPortType BelongingPortType { get; set; }

    /// <summary>
    /// 浏览记录
    /// </summary>
    public List<BrowseRecord> BrowseRecords { get; set; } = [];

    

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
