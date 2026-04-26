using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

[AutoMapFrom(typeof(CourseChapterQuery))]
[AutoMapTo(typeof(CourseChapterQuery))]
public class QueryCourseChapterViewModel : QueryDtoBase<CourseChapterQueryListViewModel>;

[AutoMapFrom(typeof(CourseChapter))]
[AutoMapTo(typeof(CourseChapter))]
public record CourseChapterQueryListViewModel : IDto
{
    public Guid Id { get; init; }

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