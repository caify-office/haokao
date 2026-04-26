namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 敏感词
/// </summary>
public class SensitiveWord : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}