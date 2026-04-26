namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

/// <summary>
/// 续读申请审核日志
/// </summary>
[Comment("续读申请审核日志")]
public class ProductExtensionAuditLog : BaseEntity<Guid>, IIncludeCreateTime, IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 关联的申请Id
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// 变更后的状态
    /// </summary>
    public ProductExtensionRequestState NewState { get; set; }

    /// <summary>
    /// 审核意见/备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 操作人Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 操作人名称 (冗余字段，便于历史追溯)
    /// </summary>
    public string CreatorName { get; set; }
}