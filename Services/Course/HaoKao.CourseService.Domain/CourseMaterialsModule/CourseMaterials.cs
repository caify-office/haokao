namespace HaoKao.CourseService.Domain.CourseMaterialsModule;

/// <summary>
/// 课程章节讲义
/// </summary>
/// better name CourseChapterHandout
public class CourseMaterials : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    public Guid CourseChapterId { get; set; }

    /// <summary>
    /// 关联的知识点Id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 讲义名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 讲义地址
    /// </summary>
    public string FileUrl { get; set; }

    /// <summary>
    /// 排序,处理上移下移,做成一个自增强的方式
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}