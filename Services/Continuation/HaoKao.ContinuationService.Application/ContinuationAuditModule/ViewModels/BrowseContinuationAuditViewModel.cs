using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;

[AutoMapFrom(typeof(ContinuationAudit))]
public record BrowseContinuationAuditViewModel : IDto
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    public string StudentName { get; init; }

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
    /// 申请人Id
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}