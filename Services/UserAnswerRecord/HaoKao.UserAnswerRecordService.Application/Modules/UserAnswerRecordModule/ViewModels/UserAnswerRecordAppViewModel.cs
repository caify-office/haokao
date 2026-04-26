using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

/// <summary>
/// 答题记录-查看结果
/// </summary>
[AutoMapFrom(typeof(UserAnswerRecord))]
public class UserAnswerRecordAppViewModel : IDto
{
    /// <summary>
    /// 标识符名称
    /// </summary>
    public string RecordIdentifierName { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 用户得分
    /// </summary>
    public decimal UserScore { get; set; }

    /// <summary>
    /// 及格分数
    /// </summary>
    public decimal PassingScore { get; set; }

    /// <summary>
    /// 试题总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 少选数
    /// </summary>
    public int LackCount => RecordQuestions.Count(x => x.JudgeResult == ScoringRuleType.Lack);

    /// <summary>
    /// 正确数
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// 试题作答记录
    /// </summary>
    public List<UserAnswerRecordQuestionAppViewModel> RecordQuestions { get; set; } = [];
}

[AutoMapFrom(typeof(UserAnswerQuestion))]
public class UserAnswerRecordQuestionAppViewModel
{
    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 试题父Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 用户得分
    /// </summary>
    public decimal UserScore { get; set; }

    /// <summary>
    /// 判题结果
    /// </summary>
    public ScoringRuleType JudgeResult { get; set; }

    /// <summary>
    /// 是否标记
    /// </summary>
    public bool WhetherMark { get; set; }

    /// <summary>
    ///题型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 用户作答
    /// </summary>
    public IReadOnlyList<UserAnswerRecordQuestionOptionAppViewModel> QuestionOptions { get; set; }
}

[AutoMapFrom(typeof(UserQuestionOption))]
public class UserAnswerRecordQuestionOptionAppViewModel : IDto
{
    /// <summary>
    /// 选项Id
    /// </summary>
    public Guid OptionId { get; set; }

    /// <summary>
    /// 回答内容
    /// </summary>
    public string OptionContent { get; set; }
}