using HaoKao.CourseService.Domain.CourseVideoNoteModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.ViewModels;

[AutoMapFrom(typeof(CourseVideoNoteQuery))]
[AutoMapTo(typeof(CourseVideoNoteQuery))]
public class QueryCourseVideoNoteViewModel : QueryDtoBase<BrowseCourseVideoNoteViewModel>
{
    /// <summary>
    /// 视频id
    /// </summary>
    public string VideoId { get; set; }
}