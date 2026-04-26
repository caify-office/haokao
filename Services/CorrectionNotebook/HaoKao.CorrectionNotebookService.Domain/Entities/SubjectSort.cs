namespace HaoKao.CorrectionNotebookService.Domain.Entities;

public sealed class SubjectSort : AggregateRoot<Guid>, IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 是否内置数据
    /// </summary>
    public bool IsBuiltIn { get; init; }

    public static SubjectSort Create(Guid userId, Guid subjectId, int priority)
    {
        return new SubjectSort
        {
            SubjectId = subjectId,
            CreatorId = userId,
            Priority = priority,
            IsBuiltIn = false
        };
    }
}