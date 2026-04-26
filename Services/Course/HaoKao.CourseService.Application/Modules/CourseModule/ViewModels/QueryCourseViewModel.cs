using HaoKao.CourseService.Domain.CourseModule;

namespace HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;

[AutoMapFrom(typeof(CourseQuery))]
[AutoMapTo(typeof(CourseQuery))]
public class QueryCourseViewModel : QueryDtoBase<BrowseCourseViewModel>
{
    /// <summary>
    /// 课程名称
    /// </summary>
    public string CourseName { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 上下架  true---上架  false-下架
    /// </summary>
    public bool? State { get; set; }

    /// <summary>
    /// 课程ids
    /// </summary>
    public string CourseIds { get; set; }

    /// <summary>
    /// 课程类型
    /// </summary>
    public CourseType? CourseType { get; set; }
}