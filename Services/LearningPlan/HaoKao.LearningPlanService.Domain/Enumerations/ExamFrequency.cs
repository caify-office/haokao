using System.ComponentModel;

namespace HaoKao.LearningPlanService.Domain.Enumerations;

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