namespace HaoKao.QuestionService.Domain.QuestionModule;

/// <summary>
/// 试题启用状态
/// </summary>
[Description("状态")]
public enum EnableState
{
    /// <summary>
    /// 禁用
    /// </summary>
    [Description("禁用")]
    Disable = 0,

    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enable = 1,
}

/// <summary>
/// 试题免费状态
/// </summary>
[Description("免费专区")]
public enum FreeState
{
    /// <summary>
    /// 否
    /// </summary>
    [Description("否")]
    No = 0,

    /// <summary>
    /// 是
    /// </summary>
    [Description("是")]
    Yes = 1,
}