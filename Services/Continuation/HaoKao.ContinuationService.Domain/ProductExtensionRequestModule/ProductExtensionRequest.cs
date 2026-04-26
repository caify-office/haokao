namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

/// <summary>
/// 课程续读申请
/// </summary>
[Comment("课程续读申请")]
public class ProductExtensionRequest : AggregateRoot<Guid>,
                                       ITenantShardingTable,
                                       IIncludeMultiTenant<Guid>,
                                       IIncludeCreateTime,
                                       IIncludeUpdateTime,
                                       IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 关联的策略Id
    /// </summary>
    public Guid PolicyId { get; set; }

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
    /// 产品过期时间 (申请时的原过期时间)
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    public string StudentName { get; set; }

    /// <summary>
    /// 申请理由 (结构化数据，保留原 Reason 字段名或改为更明确的 ReasonId)
    /// </summary>
    public Guid ReasonId { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 相关证明 (存URL集合)
    /// </summary>
    public List<string> Evidences { get; set; } = [];

    /// <summary>
    /// 当前审核状态 (快照，冗余设计)
    /// </summary>
    public ProductExtensionRequestState AuditState { get; set; }

    /// <summary>
    /// 当前审核意见 (快照，冗余设计)
    /// </summary>
    public string AuditReason { get; set; }

    /// <summary>
    /// 最后审核时间 (快照，冗余设计)
    /// </summary>
    public DateTime? AuditTime { get; set; }

    /// <summary>
    /// 最后审核人 (快照，冗余设计)
    /// </summary>
    public string AuditOperatorName { get; set; }

    /// <summary>
    /// 剩余申请次数
    /// </summary>
    public int RestOfCount { get; set; }

    /// <summary>
    /// 审核日志集合
    /// </summary>
    public List<ProductExtensionAuditLog> AuditLogs { get; set; } = [];

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