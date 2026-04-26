namespace HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

/// <summary>
/// 课程续读策略
/// </summary>
[Comment("课程续读策略")]
public class ProductExtensionPolicy : AggregateRoot<Guid>,
                                      IIncludeCreateTime,
                                      IIncludeUpdateTime,
                                      IIncludeDeleteField,
                                      ITenantShardingTable,
                                      IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 策略名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 申请开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 申请结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 延期类型
    /// </summary>
    public ExtensionType ExtensionType { get; set; }

    /// <summary>
    /// 延长天数 (当 ExtensionType 为 Duration 时使用)
    /// </summary>
    public int? ExtensionDays { get; set; }

    /// <summary>
    /// 固定的过期时间 (当 ExtensionType 为 FixedDate 时使用)
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 关联的产品集合 (JSON存储或子表)
    /// 为了更灵活，这里定义为新的值对象集合
    /// </summary>
    public List<PolicyProduct> Products { get; set; } = [];

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; set; }

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

    /// <summary>
    /// 是否删除标识
    /// </summary>
    public bool IsDelete { get; set; }
}

public record PolicyProduct
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 允许续读的次数限制
    /// </summary>
    public int MaxExtensionCount { get; init; }
}