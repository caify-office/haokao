namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

/// <summary>
/// 续读申请状态
/// </summary>
public enum ProductExtensionRequestState
{
    /// <summary>
    /// 待审核
    /// </summary>
    [Description("待审核")]
    Waiting = 0,

    /// <summary>
    /// 审核通过
    /// </summary>
    [Description("审核通过")]
    Approved = 1,

    /// <summary>
    /// 审核不通过
    /// </summary>
    [Description("审核不通过")]
    Rejected = 2,

    /// <summary>
    /// 需补充材料 (新增状态，增强灵活性)
    /// </summary>
    [Description("需补充材料")]
    NeedMoreEvidence = 3,
}