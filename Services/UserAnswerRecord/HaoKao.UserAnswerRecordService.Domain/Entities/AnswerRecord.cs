using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Domain.Entities;

[Comment("作答记录")]
public class AnswerRecord : AggregateRoot<Guid>,
                            IIncludeCreatorId<Guid>,
                            IIncludeMultiTenant<Guid>,
                            IIncludeCreateTime,
                            ITenantShardingTable
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

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
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 答题类型
    /// </summary>
    public SubmitAnswerType AnswerType { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 作答试题
    /// </summary>
    public List<AnswerQuestion> Questions { get; set; } = [];
}

[Comment("作答试题")]
public class AnswerQuestion : AggregateRoot<Guid>,
                              IIncludeCreatorId<Guid>,
                              IIncludeMultiTenant<Guid>,
                              IIncludeCreateTime,
                              ITenantShardingTable
{
    /// <summary>
    /// 作答记录Id
    /// </summary>
    public Guid RecordId { get; set; }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 用户作答
    /// </summary>
    public List<UserAnswer> UserAnswers { get; set; } = [];

    /// <summary>
    /// 判题结果
    /// </summary>
    public ScoringRuleType JudgeResult { get; set; }

    /// <summary>
    /// 是否标记
    /// </summary>
    public bool WhetherMark { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 试题父Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// 用户作答
/// </summary>
/// <param name="Content">回答内容</param>
public record UserAnswer(string Content);