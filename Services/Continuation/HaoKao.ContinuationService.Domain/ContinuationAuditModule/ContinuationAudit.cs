namespace HaoKao.ContinuationService.Domain.ContinuationAuditModule;

/// <summary>
/// 续读审核
/// </summary>
[Comment("续读审核")]
public class ContinuationAudit : AggregateRoot<Guid>,
                                 ITenantShardingTable,
                                 IIncludeMultiTenant<Guid>,
                                 IIncludeCreateTime,
                                 IIncludeUpdateTime,
                                 IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 续读配置Id
    /// </summary>
    public Guid SetupId { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; set; }

    /// <summary>
    /// 协议名称
    /// </summary>
    public string AgreementName { get; set; }

    /// <summary>
    /// 产品的赠品Id集合
    /// </summary>
    public List<string> ProductGifts { get; set; } = [];

    /// <summary>
    /// 产品过期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    public string StudentName { get; set; }

    /// <summary>
    /// 续读原因
    /// </summary>
    public Guid Reason { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 相关证明
    /// </summary>
    public List<string> Evidences { get; set; } = [];

    /// <summary>
    /// 审核状态
    /// </summary>
    public AuditState AuditState { get; set; }

    /// <summary>
    /// 不通过原因
    /// </summary>
    public string AuditReason { get; set; }

    /// <summary>
    /// 剩余申请次数
    /// </summary>
    public int RestOfCount { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 申请人Id
    /// </summary>
    public Guid CreatorId { get; set; }
}