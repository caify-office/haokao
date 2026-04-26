using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public class FillIn : QuestionBase<FillInOption>
{
    public FillIn()
    {
        QuestionTypeId = QuestionType.FillIn;
        QuestionTypeName = "填空题";
        Code = 6;
        IncludeAnalysis = true;
        Option = new FillInOption();
        ScoringRules = new Dictionary<ScoringRuleType, decimal>
        {
            { ScoringRuleType.Correct, 2 },
            { ScoringRuleType.Missing, 0 },
            { ScoringRuleType.Wrong, 0 },
            { ScoringRuleType.Vague, 0 },
        };
    }

    public override ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers)
    {
        return null;
    }
}