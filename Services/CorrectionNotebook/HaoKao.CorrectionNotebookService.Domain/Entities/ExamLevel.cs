namespace HaoKao.CorrectionNotebookService.Domain.Entities;

/// <summary>
/// 考试级别实体类
/// </summary>
public sealed class ExamLevel : AggregateRoot<Guid>,
                                IIncludeCreatorId<Guid>,
                                IIncludeCreateTime
{
    /// <summary>
    /// 考试级别名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 是否内置数据
    /// </summary>
    public bool IsBuiltIn { get; init; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public Guid ParentId { get; init; }

    /// <summary>
    /// 科目集合
    /// </summary>
    public List<Subject> Subjects { get; set; } = [];

    internal static ExamLevel Create(string name, Guid parentId, Guid creatorId)
    {
        return new ExamLevel
        {
            Name = name,
            IsBuiltIn = false,
            CreatorId = creatorId,
            CreateTime = DateTime.Now,
            ParentId = parentId,
        };
    }

    public void AddSubject(string name, Guid userId, int sort)
    {
        var subject = Subject.Create(Id, name, userId, sort);
        Subjects.Add(subject);
    }
}