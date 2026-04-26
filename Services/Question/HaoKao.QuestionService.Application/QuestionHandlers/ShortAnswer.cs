using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public class ShortAnswer : QuestionBase<ShortAnswerQuestionOption>
{
    public ShortAnswer()
    {
        QuestionTypeId = QuestionType.ShortAnswer;
        QuestionTypeName = "问答题";
        Code = 5;
        IncludeAnalysis = true;
        Option = new ShortAnswerQuestionOption();
        ScoringRules = new Dictionary<ScoringRuleType, decimal>
        {
            { ScoringRuleType.Missing, 0 },
            { ScoringRuleType.Subjective, 0 },
        };
    }

    public override ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers)
    {
        var resultType = userAnswers.Length == 0 ? ScoringRuleType.Missing : ScoringRuleType.Subjective;
        return new ScoringResult
        {
            ScoringType = resultType,
            Score = scoringRules.TryGetValue(resultType, out var val) ? val : 0
        };
    }
}