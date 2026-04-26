using HaoKao.Common.Enums;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

/// <summary>
/// 判分结果
/// </summary>
public class ScoringResult
{
    /// <summary>
    /// 答题情况
    /// </summary>
    public ScoringRuleType ScoringType { get; set; }

    /// <summary>
    /// 得分
    /// </summary>
    public decimal Score { get; set; }
}