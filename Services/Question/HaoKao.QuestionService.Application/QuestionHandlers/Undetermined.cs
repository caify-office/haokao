using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public class Undetermined : ChoiceQuestionBase
{
    public Undetermined()
    {
        QuestionTypeId = QuestionType.Undetermined;
        QuestionTypeName = "不定项选择题";
        Code = 3;
        IncludeAnalysis = true;
        Option = new ChoiceQuestionOption();
        ScoringRules = new Dictionary<ScoringRuleType, decimal>
        {
            { ScoringRuleType.Correct, 2 },
            { ScoringRuleType.Missing, 0 },
            { ScoringRuleType.Wrong, 0 },
            { ScoringRuleType.Lack, 0 },
            { ScoringRuleType.Excessive, 0 },
        };
    }

    public override ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers)
    {
        return MultipleChoiceScoringHandler(scoringRules, userAnswers);
    }
}