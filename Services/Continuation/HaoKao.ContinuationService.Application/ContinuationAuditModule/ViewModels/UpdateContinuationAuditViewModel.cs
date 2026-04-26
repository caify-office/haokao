using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;

[AutoMapTo(typeof(UpdateContinuationAuditCommand))]
public record UpdateContinuationAuditViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 审核状态
    /// </summary>
    [DisplayName("审核状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public AuditState AuditState { get; init; }

    /// <summary>
    /// 不通过原因
    /// </summary>
    [DisplayName("不通过原因")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string AuditReason { get; init; }
}