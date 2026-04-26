using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

[AutoMapFrom(typeof(CourseChapter))]
public record BrowseCourseChapterViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 父id
    /// </summary>
    public Guid ParentId { get; init; }

    /// <summary>
    /// 关联的课程id
    /// </summary>
    public Guid CourseId { get; init; }
}