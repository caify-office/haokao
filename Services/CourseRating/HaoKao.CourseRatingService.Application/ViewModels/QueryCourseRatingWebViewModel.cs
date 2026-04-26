using HaoKao.CourseRatingService.Domain.Queries;

namespace HaoKao.CourseRatingService.Application.ViewModels;

[AutoMapFrom(typeof(CourseRatingQuery))]
[AutoMapTo(typeof(CourseRatingQuery))]
public class QueryCourseRatingWebViewModel : QueryDtoBase<QueryCourseRatingListWebViewModel>
{
    /// <summary>
    /// 课程Id
    /// </summary>
    [JsonIgnore]
    public string CourseId { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.CourseRating))]
[AutoMapTo(typeof(Domain.Entities.CourseRating))]
public record QueryCourseRatingListWebViewModel : IDto
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; init; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string Comment { get; init; }

    /// <summary>
    /// 评价级别
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool Sticky { get; init; }

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 评价人Id
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; init; }
}