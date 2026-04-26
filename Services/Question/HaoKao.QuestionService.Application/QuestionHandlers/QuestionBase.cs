using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;
using Newtonsoft.Json;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public abstract class QuestionBase<TOption> : IQuestion<TOption> where TOption : AnswerAreaBase, new()
{
    public Guid QuestionTypeId { get; protected init; }

    public string QuestionTypeName { get; protected init; }

    public int Code { get; protected init; }

    public bool IncludeAnalysis { get; protected init; }

    public TOption Option { get; protected init; }

    protected List<TOption> QuestionOptions { get; set; }

    public Dictionary<ScoringRuleType, decimal> ScoringRules { get; protected init; }

    public void SetQuestionOption(string optionsJsonStr)
    {
        QuestionOptions = JsonConvert.DeserializeObject<List<TOption>>(optionsJsonStr);
    }

    public abstract ScoringResult HandleScoring(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers);
}

public abstract class ChoiceQuestionBase : QuestionBase<ChoiceQuestionOption>
{
    protected ScoringResult SingleChoiceScoringHandler(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers)
    {
        // 未作答
        if (userAnswers.Length == 0)
        {
            const ScoringRuleType missing = ScoringRuleType.Missing;
            return new ScoringResult
            {
                ScoringType = missing,
                Score = scoringRules[missing],
            };
        }

        // 单选题取第一个作答
        var answer = userAnswers[0];

        // 获取标准答案
        var correctAnswer = QuestionOptions.FirstOrDefault(x => x.IsAnswer);

        // 答对
        if (answer.Id == correctAnswer?.Id)
        {
            const ScoringRuleType correct = ScoringRuleType.Correct;
            return new ScoringResult
            {
                ScoringType = correct,
                Score = scoringRules[correct],
            };
        }

        // 其它情况为答错
        const ScoringRuleType wrong = ScoringRuleType.Wrong;
        return new ScoringResult
        {
            ScoringType = wrong,
            Score = scoringRules[wrong],
        };
    }

    protected ScoringResult MultipleChoiceScoringHandler(Dictionary<ScoringRuleType, decimal> scoringRules, params UserAnswerContent[] userAnswers)
    {
        // 未作答
        if (userAnswers.Length == 0)
        {
            const ScoringRuleType missing = ScoringRuleType.Missing;
            return new ScoringResult
            {
                ScoringType = missing,
                Score = scoringRules[missing],
            };
        }

        // 获取标准答案
        var correctAnswers = QuestionOptions.Where(x => x.IsAnswer).Select(x => x.Id);
        var correctAnswerCount = correctAnswers.Count();

        // 获取答对的用户答案
        var correctUserAnswers = correctAnswers.Intersect(userAnswers.Select(x => x.Id));
        var correctUserAnswerCount = correctUserAnswers.Count();
        var userAnswerCount = userAnswers.Length;

        // 如果用户作答正确的个数和标准答案的个数是一样的，则回答正确
        if (userAnswerCount == correctUserAnswerCount && userAnswerCount == correctAnswerCount)
        {
            // 答对: 命中全部的正确选项
            const ScoringRuleType correct = ScoringRuleType.Correct;
            return new ScoringResult
            {
                ScoringType = correct,
                Score = scoringRules[correct],
            };
        }

        // 如果用户作答正确的个数和标准答案的个数是一样的，且用户作答数小于标准答案数，则是少选
        if (userAnswerCount == correctUserAnswerCount && userAnswerCount < correctAnswerCount)
        {
            const ScoringRuleType lack = ScoringRuleType.Lack;
            return new ScoringResult
            {
                ScoringType = lack,
                Score = scoringRules[lack] * userAnswerCount, //少选一项固定得分,多个需要累计
            };
        }

        // 答错: 非以上情况都算答错
        const ScoringRuleType wrong = ScoringRuleType.Wrong;
        return new ScoringResult
        {
            ScoringType = wrong,
            Score = scoringRules[wrong],
        };
    }
}