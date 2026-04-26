using HaoKao.CourseService.Domain.CourseModule;

namespace HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;

[AutoMapTo(typeof(UpdateCourseMaterialsPackageUrlCommand))]
public class UpdateCourseMaterialsPackageUrlViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    [DisplayName("主键")]
    public Guid Id { get; init; }

    /// <summary>
    /// 上传课程讲义包名称
    /// </summary>
    public string CourseMaterialsPackageName { get; set; }

    /// <summary>
    /// 课程讲义包url
    /// </summary>
    [DisplayName("课程讲义包url")]
    public string CourseMaterialsPackageUrl { get; init; }
}