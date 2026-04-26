namespace HaoKao.QuestionService.Domain.QuestionModule;

public class UnionQuestion : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    public Guid? ChapterNodeId { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    public Guid QuestionCategoryId { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 父题目Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}