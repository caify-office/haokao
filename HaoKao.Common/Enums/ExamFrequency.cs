using System.ComponentModel;

namespace HaoKao.Common.Enums;

/// <summary>
/// 考试频率
/// </summary>
public enum ExamFrequency
{
    /// <summary>
    /// 高频
    /// </summary>
    [Description("高频")]
    High = 0,

    /// <summary>
    /// 中频
    /// </summary>
    [Description("中频")]
    Medium = 1,

    /// <summary>
    /// 低频
    /// </summary>
    [Description("低频")]
    Low = 2,
}