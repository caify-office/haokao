using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public class SingleChoice : ChoiceQuestionBase
{
    public SingleChoice()
    {
        QuestionTypeId = QuestionType.SingleChoice;
        QuestionTypeName = "单选题";
        Code = 1;
        IncludeAnalysis = true;
        Option = new ChoiceQuestionOption();
        ScoringRules = new Dictionary<ScoringRuleType, decimal>
        {
            { ScoringRuleType.Correct, 2 },
            { ScoringRuleType.Missing, 0 },
            { ScoringRuleType.Wrong, 0 },
        };
    }

    public override ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers)
    {
        return SingleChoiceScoringHandler(scoringRules, userAnswers);
    }
}