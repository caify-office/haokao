using System.ComponentModel;

namespace HaoKao.Common.Enums;

/// <summary>
/// 评分规则
/// </summary>
[Description("评分规则")]
public enum ScoringRuleType
{
    /// <summary>
    /// 答对
    /// </summary>
    [Description("答对")]
    Correct,

    /// <summary>
    /// 答错
    /// </summary>
    [Description("答错")]
    Wrong,

    /// <summary>
    /// 不作答
    /// </summary>
    [Description("不作答")]
    Missing,

    /// <summary>
    /// 少选
    /// </summary>
    [Description("少选")]
    Lack,

    /// <summary>
    /// 多选
    /// </summary>
    [Description("多选")]
    Excessive,

    /// <summary>
    /// 模糊词
    /// </summary>
    [Description("模糊词")]
    Vague,

    /// <summary>
    /// 主观题(不评分)
    /// </summary>
    [Description("主观题")]
    Subjective = -1,
}