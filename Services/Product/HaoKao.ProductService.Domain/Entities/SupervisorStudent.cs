namespace HaoKao.ProductService.Domain.Entities;

/// <summary>
/// 督学学员
/// </summary>
public class SupervisorStudent : AggregateRoot<Guid>,
                                 IIncludeMultiTenant<Guid>,
                                 IIncludeCreateTime
{
    /// <summary>
    /// 督学班级id
    /// </summary>
    public Guid SupervisorClassId { get; set; }

    /// <summary>
    /// 所属班级
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public SupervisorClass SupervisorClass { get; set; }

    /// <summary>
    /// 注册用户id
    /// </summary>
    public Guid RegisterUserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    ///  最大听课时长
    /// </summary>
    public float MaxProgress { get; set; }

    /// <summary>
    /// 总时长
    /// </summary>
    public float CourseDuration { get; set; }

    /// <summary>
    /// 总学习进度
    /// </summary>
    public float CourseRatio { get; set; }

    /// <summary>
    /// 已学完课程数
    /// </summary>
    public int IsEndCourseCount { get; set; }

    /// <summary>
    /// 课程数
    /// </summary>
    public int CourseCount { get; set; }

    /// <summary>
    /// 最近学习时间
    /// </summary>
    public DateTime? LastLearnTime { get; set; }

    /// <summary>
    /// 统计时间 （每一天只统计一次）
    /// </summary>
    public DateTime? StatisticsTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}