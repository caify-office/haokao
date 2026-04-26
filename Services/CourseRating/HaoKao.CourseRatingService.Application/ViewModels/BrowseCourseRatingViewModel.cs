namespace HaoKao.CourseRatingService.Application.ViewModels;

[AutoMapTo(typeof(Domain.Entities.CourseRating))]
[AutoMapFrom(typeof(Domain.Entities.CourseRating))]
public record BrowseCourseRatingViewModel : IDto
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; init; }

    /// <summary>
    /// 课程名称
    /// </summary>
    public string CourseName { get; init; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string Comment { get; init; }

    /// <summary>
    /// 评价级别
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 评价人Id
    /// </summary>
    public Guid CreatorId { get; init; }
}