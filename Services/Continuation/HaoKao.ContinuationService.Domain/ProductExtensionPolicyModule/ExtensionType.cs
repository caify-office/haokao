namespace HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

/// <summary>
/// 续读延期类型
/// </summary>
public enum ExtensionType
{
    /// <summary>
    /// 固定日期
    /// </summary>
    [Description("固定日期")]
    FixedDate = 0,

    /// <summary>
    /// 相对天数
    /// </summary>
    [Description("相对天数")]
    Duration = 1,
}