using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;

[AutoMapFrom(typeof(ContinuationAudit))]
public record BrowseContinuationAuditWebViewModel : IDto
{
    /// <summary>
    /// 续读原因
    /// </summary>
    public Guid Reason { get; init; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// 相关证明
    /// </summary>
    public List<string> Evidences { get; init; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public AuditState AuditState { get; init; }

    /// <summary>
    /// 不通过原因
    /// </summary>
    public string AuditReason { get; init; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpiryTime { get; init; }
}