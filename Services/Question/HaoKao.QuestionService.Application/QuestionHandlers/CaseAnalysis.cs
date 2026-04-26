using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public class CaseAnalysis : QuestionBase<CaseAnalysisOption>
{
    public CaseAnalysis()
    {
        QuestionTypeId = QuestionType.CaseAnalysis;
        QuestionTypeName = "案例分析题";
        Code = 7;
        IncludeAnalysis = true;
        Option = null;
        ScoringRules = new Dictionary<ScoringRuleType, decimal>();
    }

    public override ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers)
    {
        return null;
    }
}