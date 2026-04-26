namespace HaoKao.UserAnswerRecordService.Domain.Entities;

[Comment("章节知识点作答记录")]
public class ChapterAnswerRecord : AggregateRoot<Guid>,
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
    /// 分类Id
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 小节Id
    /// </summary>
    public Guid SectionId { get; set; }

    /// <summary>
    /// 知识点Id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 作答记录Id
    /// </summary>
    public Guid AnswerRecordId { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 答题记录
    /// </summary>
    public AnswerRecord AnswerRecord { get; set; }
}

/// <summary>
/// 章节答题统计
/// </summary>
/// <param name="ChapterCount">已练章节</param>
/// <param name="AnswerCount">答题数</param>
/// <param name="CorrectCount">正确题数</param>
/// <param name="QuestionCount">总题数</param>
/// <param name="ElapsedTime">总耗时</param>
public record ChapterRecordStat(int ChapterCount, int AnswerCount, int CorrectCount, int QuestionCount)
{
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 每道题耗时
    /// </summary>
    public long QuestionTime => ElapsedTime > 0 && AnswerCount > 0 ? ElapsedTime / AnswerCount : 0;

    /// <summary>
    /// 正确率
    /// </summary>
    public double CorrectRate => Math.Round(AnswerCount == 0 ? 0 : 100.0 * CorrectCount / AnswerCount, 2);
}