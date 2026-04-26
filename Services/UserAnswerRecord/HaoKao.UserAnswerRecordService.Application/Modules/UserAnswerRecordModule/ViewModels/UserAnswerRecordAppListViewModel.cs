using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

[AutoMapFrom(typeof(UserAnswerRecord))]
public class UserAnswerRecordChapterListAppViewModel : IDto
{
    /// <summary>
    /// 记录Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    [JsonPropertyName("chapterNodeId")]
    public Guid RecordIdentifier { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 作答数
    /// </summary>
    public int AnswerCount { get; set; }

    /// <summary>
    /// 正确数
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}

/// <summary>
/// 答题记录-试卷列表
/// </summary>
[AutoMapFrom(typeof(UserAnswerRecord))]
public class UserAnswerRecordPaperListAppViewModel : IDto
{
    /// <summary>
    /// 记录Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 试卷Id
    /// </summary>
    [JsonPropertyName("paperId")]
    public Guid RecordIdentifier { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal UserScore { get; set; }

    /// <summary>
    /// 及格分数
    /// </summary>
    public decimal PassingScore { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 作答数
    /// </summary>
    public int AnswerCount { get; set; }

    /// <summary>
    /// 正确数
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}