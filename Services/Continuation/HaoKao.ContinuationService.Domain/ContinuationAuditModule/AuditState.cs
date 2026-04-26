namespace HaoKao.ContinuationService.Domain.ContinuationAuditModule;

public enum AuditState
{
    /// <summary>
    /// 待审核
    /// </summary>
    [Description("待审核")]
    InAudit,

    /// <summary>
    /// 审核通过
    /// </summary>
    [Description("审核通过")]
    Pass,

    /// <summary>
    /// 审核不通过
    /// </summary>
    [Description("审核不通过")]
    NotPass,
}