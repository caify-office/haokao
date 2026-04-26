namespace HaoKao.ContinuationService.Domain.ContinuationSetupModule;

/// <summary>
/// 续读配置
/// </summary>
[Comment("续读配置")]
public class ContinuationSetup : AggregateRoot<Guid>,
                                 IIncludeCreateTime,
                                 IIncludeUpdateTime,
                                 IIncludeDeleteField,
                                 ITenantShardingTable,
                                 IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 续读申请开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 续读申请结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 续读后的到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 产品集合
    /// </summary>
    public IReadOnlyList<SetupProduct> Products { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }

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

public record SetupProduct
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
    /// 续读次数
    /// </summary>
    public int Continuation { get; init; }
}