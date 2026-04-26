using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public interface IScocingable
{
    /// <summary>
    /// 开始判分
    /// </summary>
    /// <param name="userAnswers">作答回容</param>
    /// <param name="scoringRules">当前试题的评分规则</param>
    /// <returns></returns>
    ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers);
}

public interface IQuestion
{
    /// <summary>
    /// 题型Id
    /// </summary>
    Guid QuestionTypeId { get; }

    /// <summary>
    /// 题型名称
    /// </summary>
    string QuestionTypeName { get; }

    /// <summary>
    /// 排序
    /// </summary>
    int Code { get; }

    /// <summary>
    /// 评分规则
    /// </summary>
    Dictionary<ScoringRuleType, decimal> ScoringRules { get; }

    /// <summary>
    /// 试题选项赋值
    /// </summary>
    /// <param name="optionsJsonStr"></param>
    void SetQuestionOption(string optionsJsonStr);

    /// <summary>
    /// 开始判分
    /// </summary>
    /// <param name="userAnswers">作答回容</param>
    /// <param name="scoringRules">当前试题的评分规则</param>
    /// <returns></returns>
    ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers);
}

/// <summary>
/// 试题接口
/// </summary>
public interface IQuestion<out TOption> : IQuestion where TOption : AnswerAreaBase, new()
{
    /// <summary>
    /// 定义答题区
    /// </summary>
    TOption Option { get; }

    /// <summary>
    /// 是否包含解析
    /// </summary>
    bool IncludeAnalysis { get; }
}