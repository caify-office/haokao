using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

[AutoMapTo(typeof(CourseChapter))]
public record UpdateCourseChapterViewModel : CreateCourseChapterViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    [DisplayName("Id")]
    public Guid Id { get; init; }
}