using HaoKao.CourseService.Domain.CourseModule;

namespace HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;

[AutoMapFrom(typeof(Course))]
[AutoMapTo(typeof(Course))]
public record BrowseCourseViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 课程名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 主讲老师集合
    /// </summary>
    public string TeacherJson { get; init; }

    /// <summary>
    /// 年份
    /// </summary>
    public string Year { get; init; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; init; }

    /// <summary>
    /// 预计更新时间
    /// </summary>
    public string UpdateTimeDesc { get; init; }

    /// <summary>
    /// 上传课程讲义包名称
    /// </summary>
    public string CourseMaterialsPackageName { get; set; }

    /// <summary>
    /// 上传课程讲义包Url
    /// </summary>
    public string CourseMaterialsPackageUrl { get; set; }

    /// <summary>
    /// 上架状态 false-下架 true-上架
    /// </summary>
    public bool State { get; init; }

    /// <summary>
    /// 课程类型
    /// </summary>
    public CourseType CourseType { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; init; }
}