using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Domain.Entities;

public class UnionAnswerRecord : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, IIncludeCreatorId<Guid>, IIncludeCreateTime
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 答题标识符 章节Id，或试卷Id，每日一练和消灭错题 为Guid.Empty
    /// </summary>
    public Guid RecordIdentifier { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    public Guid QuestionCategoryId { get; set; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}

public class UnionAnswerQuestion : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 答题记录Id
    /// </summary>
    public Guid UnionAnswerRecordId { get; set; }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 试题父Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 判题结果
    /// </summary>
    public ScoringRuleType JudgeResult { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}