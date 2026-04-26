namespace HaoKao.CourseFeatureService.Domain;

/// <summary>
/// 课程特色服务
/// </summary>
[Comment("课程特色服务")]
public class CourseFeature : AggregateRoot<Guid>,
                             IIncludeCreatorId<Guid>,
                             IIncludeMultiTenant<Guid>,
                             IIncludeCreateTime,
                             IIncludeUpdateTime
{
    /// <summary>
    ///服务名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 服务内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    public string IconUrl { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }
}