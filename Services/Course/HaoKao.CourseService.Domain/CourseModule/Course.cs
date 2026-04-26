namespace HaoKao.CourseService.Domain.CourseModule;

/// <summary>
/// 课程
/// </summary>
public class Course : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 课程名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 主讲老师集合
    /// </summary>
    public string TeacherJson { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public string Year { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 预计更新时间
    /// </summary>
    public string UpdateTimeDesc { get; set; }

    /// <summary>
    /// 上传课程讲义包名称
    /// </summary>
    public string CourseMaterialsPackageName { get; set; }

    /// <summary>
    /// 上传课程讲义包Url
    /// </summary>
    public string CourseMaterialsPackageUrl { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// 课程类型
    /// </summary>
    public CourseType CourseType { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// 课程类型
/// </summary>
public enum CourseType
{
    /// <summary>
    /// 阶段课程
    /// </summary>
    StageCourse = 0,

    /// <summary>
    /// 智辅课程
    /// </summary>
    IntelligenceCourse = 1,
}