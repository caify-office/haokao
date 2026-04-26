namespace HaoKao.CourseService.Domain.CourseChapterModule;

/// <summary>
/// 课程章节
/// </summary>
public class CourseChapter : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父id
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// 关联的课程id
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 是否是叶子节点
    /// </summary>
    public bool IsLeaf { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}