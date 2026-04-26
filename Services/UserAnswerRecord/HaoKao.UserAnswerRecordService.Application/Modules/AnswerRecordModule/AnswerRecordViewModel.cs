using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Application.Helpers;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;

[AutoMapFrom(typeof(AnswerRecord))]
public record AnswerRecordViewModel : IDto
{
    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; init; }

    /// <summary>
    /// 作答数
    /// </summary>
    public int AnswerCount { get; init; }

    /// <summary>
    /// 正确数
    /// </summary>
    public int CorrectCount { get; init; }

    /// <summary>
    /// 正确率
    /// </summary>
    public int CorrectRate => PercentageHelper.CalculatePercentage(CorrectCount, QuestionCount);

    /// <summary>
    /// 少选数
    /// </summary>
    public int LackCount => Questions.Count(x => x.JudgeResult == ScoringRuleType.Lack);

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 作答试题
    /// </summary>
    public IReadOnlyList<AnswerQuestionViewModel> Questions { get; init; } = [];
}

[AutoMapFrom(typeof(AnswerQuestion))]
public record AnswerQuestionViewModel : IDto
{
    /// <summary>
    /// 作答记录Id
    /// </summary>
    public Guid RecordId { get; init; }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; init; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    public Guid QuestionTypeId { get; init; }

    /// <summary>
    /// 判题结果
    /// </summary>
    public ScoringRuleType JudgeResult { get; init; }

    /// <summary>
    /// 是否标记
    /// </summary>
    public bool WhetherMark { get; init; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 试题父Id
    /// </summary>
    public Guid? ParentId { get; init; }

    /// <summary>
    /// 用户作答
    /// </summary>
    public IReadOnlyList<UserAnswerViewModel> UserAnswers { get; init; } = [];
}

[AutoMapFrom(typeof(UserAnswer))]
public record UserAnswerViewModel : IDto
{
    /// <summary>
    /// 回答内容
    /// </summary>
    public string Content { get; init; }
}