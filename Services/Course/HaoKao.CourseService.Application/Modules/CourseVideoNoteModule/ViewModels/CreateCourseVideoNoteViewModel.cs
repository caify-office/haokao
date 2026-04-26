using HaoKao.CourseService.Domain.CourseVideoNoteModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.ViewModels;

[AutoMapTo(typeof(CreateCourseVideoNoteCommand))]
public record CreateCourseVideoNoteViewModel : IDto
{
    /// <summary>
    /// 视频id
    /// </summary>
    [DisplayName("视频id")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string VideoId { get; init; }

    /// <summary>
    /// 视频时间节点
    /// </summary>
    [DisplayName("视频时间节点")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal TimeNode { get; init; }

    /// <summary>
    /// 笔记类型
    /// </summary>
    [DisplayName("笔记类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public CourseVideoNoteType CourseVideoNoteType { get; init; }

    /// <summary>
    /// 笔记内容
    /// </summary>
    [DisplayName("笔记内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string NoteContent { get; init; }
}