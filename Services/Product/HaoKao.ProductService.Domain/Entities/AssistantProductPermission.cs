namespace HaoKao.ProductService.Domain.Entities;

/// <summary>
/// 智辅产品权限
/// </summary>
public class AssistantProductPermission : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 课程阶段Id
    /// </summary>
    public Guid CourseStageId { get; set; }

    /// <summary>
    /// 课程阶段名称
    /// </summary>
    public string CourseStageName { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 智辅产品权限内容
    /// </summary>
    public ICollection<AssistantProductPermissionContent> AssistantProductPermissionContents { get; set; } = new List<AssistantProductPermissionContent>();

    /// <summary>
    /// 租户
    /// </summary>
    public Guid TenantId { get; set; }
}