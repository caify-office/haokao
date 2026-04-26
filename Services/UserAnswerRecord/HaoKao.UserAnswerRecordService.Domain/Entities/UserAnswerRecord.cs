using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Domain.Entities;

/// <summary>
/// 答题记录
/// </summary>
[Comment("答题记录")]
public class UserAnswerRecord : AggregateRoot<Guid>,
                                IIncludeCreatorId<Guid>,
                                IIncludeCreateTime,
                                IIncludeMultiTenant<Guid>,
                                ITenantShardingTable,
                                IYearShardingTable
{
    /// <summary>
    /// 答题类型
    /// </summary>
    public SubmitAnswerType AnswerType { get; set; }

    /// <summary>
    /// 答题标识符 章节Id，或试卷Id，每日一练和消灭错题 为Guid.Empty
    /// </summary>
    public Guid RecordIdentifier { get; set; }

    /// <summary>
    /// 标识符名称
    /// </summary>
    public string RecordIdentifierName { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    public Guid QuestionCategoryId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

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
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 答题记录明细
    /// </summary>
    public List<UserAnswerQuestion> RecordQuestions { get; set; } = [];
}