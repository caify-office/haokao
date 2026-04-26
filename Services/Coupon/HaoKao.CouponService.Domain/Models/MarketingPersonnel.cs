using System;

namespace HaoKao.CouponService.Domain.Models;

public class MarketingPersonnel : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string TelPhone { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建人id
    /// </summary>
    public Guid CreatorId { get; set; }
}