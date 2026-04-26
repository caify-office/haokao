namespace HaoKao.PaperService.Domain.Enumerations;

/// <summary>
/// 发布状态
/// </summary>
[Description("发布状态")]
public enum StateEnum
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 未发布
    /// </summary>
    [Description("未发布")]
    UnPublish = 1,

    /// <summary>
    /// 已发布
    /// </summary>
    [Description("已发布")]
    Published = 2,
}