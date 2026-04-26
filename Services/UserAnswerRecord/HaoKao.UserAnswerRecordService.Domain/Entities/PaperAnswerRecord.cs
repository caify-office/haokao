namespace HaoKao.UserAnswerRecordService.Domain.Entities;

[Comment("试卷作答记录")]
public class PaperAnswerRecord : AggregateRoot<Guid>,
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
    /// 试卷Id
    /// </summary>
    public Guid PaperId { get; set; }

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
    /// 作答时长(秒)
    /// </summary>
    public long ElapsedTime { get; set; }

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