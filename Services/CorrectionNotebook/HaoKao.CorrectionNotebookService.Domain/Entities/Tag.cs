namespace HaoKao.CorrectionNotebookService.Domain.Entities;

public sealed class Tag : AggregateRoot<Guid>,
                          IIncludeCreatorId<Guid>,
                          IIncludeCreateTime
{
    /// <summary>
    /// 标签分类
    /// </summary>
    public string Category { get; init; }

    /// <summary>
    /// 标签名称
    /// </summary>
    public string Name { get; init; }

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

    internal static Tag Create(string name, Guid creatorId)
    {
        return new Tag
        {
            Category = "自定义标签",
            Name = name,
            IsBuiltIn = false,
            CreatorId = creatorId,
            CreateTime = DateTime.Now,
        };
    }
}