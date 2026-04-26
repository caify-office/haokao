using HaoKao.CourseService.Domain.CourseVideoNoteModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.ViewModels;

[AutoMapTo(typeof(CourseVideoNote))]
[AutoMapFrom(typeof(CourseVideoNote))]
public record BrowseCourseVideoNoteViewModel : IDto
{
    public Guid Id { get; init; }

    /// <summary>
    /// 视频id
    /// </summary>
    public string VideoId { get; init; }

    /// <summary>
    /// 视频时间节点
    /// </summary>
    public decimal TimeNode { get; init; }

    /// <summary>
    /// 笔记类型
    /// </summary>
    public CourseVideoNoteType CourseVideoNoteType { get; init; }

    /// <summary>
    /// 笔记内容
    /// </summary>
    public string NoteContent { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; init; }
}