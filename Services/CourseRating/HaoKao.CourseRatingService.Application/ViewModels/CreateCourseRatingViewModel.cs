using HaoKao.CourseRatingService.Domain.Commands;

namespace HaoKao.CourseRatingService.Application.ViewModels;

[AutoMapTo(typeof(CreateCourseRatingCommand))]
public record CreateCourseRatingViewModel : IDto
{
    /// <summary>
    /// 课程Id
    /// </summary>
    [DisplayName("课程Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseId { get; init; }

    /// <summary>
    /// 课程名称
    /// </summary>
    [DisplayName("课程名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string CourseName { get; init; }

    /// <summary>
    /// 评价内容
    /// </summary>
    [DisplayName("评价内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(300, ErrorMessage = "{0}长度不能大于{1}")]
    public string Comment { get; init; }

    /// <summary>
    /// 评价级别
    /// </summary>
    [DisplayName("评价级别")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Rating { get; init; }

    /// <summary>
    /// 昵称名称
    /// </summary>
    [DisplayName("昵称名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string NickName { get; init; }

    /// <summary>
    /// 用户头像
    /// </summary>
    [DisplayName("用户头像")]
    public string Avatar { get; init; }
}