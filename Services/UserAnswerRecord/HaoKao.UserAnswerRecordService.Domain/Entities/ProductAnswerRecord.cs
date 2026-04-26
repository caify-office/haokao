using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Domain.Entities;

public class ProductChapterAnswerRecord : AggregateRoot<Guid>,
                                          IIncludeCreatorId<Guid>,
                                          IIncludeMultiTenant<Guid>,
                                          IIncludeCreateTime,
                                          ITenantShardingTable
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

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
    /// 答题日期
    /// </summary>
    public DateOnly CreateDate { get; set; }

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

[Comment("产品试卷作答记录")]
public class ProductPaperAnswerRecord : AggregateRoot<Guid>,
                                        IIncludeCreatorId<Guid>,
                                        IIncludeMultiTenant<Guid>,
                                        IIncludeCreateTime,
                                        ITenantShardingTable
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

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
    /// 试卷总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 答题日期
    /// </summary>
    public DateOnly CreateDate { get; set; }

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

[Comment("产品知识点作答记录")]
public class ProductKnowledgeAnswerRecord : AggregateRoot<Guid>,
                                            IIncludeCreatorId<Guid>,
                                            IIncludeMultiTenant<Guid>,
                                            IIncludeCreateTime,
                                            ITenantShardingTable
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

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
    /// 答题日期
    /// </summary>
    public DateOnly CreateDate { get; set; }

    /// <summary>
    /// 正确率
    /// </summary>
    public int CorrectRate { get; set; }

    /// <summary>
    /// 掌握程度
    /// </summary>
    public MasteryLevel MasteryLevel { get; set; }

    /// <summary>
    /// 考试频率
    /// </summary>
    public ExamFrequency ExamFrequency { get; set; }

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
/// 掌握程度
/// </summary>
public enum MasteryLevel
{
    /// <summary>
    /// 未掌握
    /// </summary>
    [Description("未掌握")]
    NotMastered = 0,

    /// <summary>
    /// 待加强
    /// </summary>
    [Description("待加强")]
    NeedsImprovement = 1,

    /// <summary>
    /// 已掌握
    /// </summary>
    [Description("已掌握")]
    Mastered = 2
}