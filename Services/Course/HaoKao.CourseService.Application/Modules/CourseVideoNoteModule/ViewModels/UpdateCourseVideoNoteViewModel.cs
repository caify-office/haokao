using HaoKao.CourseService.Domain.CourseVideoNoteModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.ViewModels;

[AutoMapTo(typeof(UpdateCourseVideoNoteCommand))]
public record UpdateCourseVideoNoteViewModel : CreateCourseVideoNoteViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }
}