using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;

[AutoMapFrom(typeof(ContinuationAudit))]
[AutoMapTo(typeof(ContinuationAudit))]
public record QueryContinuationAuditListWebViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public AuditState AuditState { get; init; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}