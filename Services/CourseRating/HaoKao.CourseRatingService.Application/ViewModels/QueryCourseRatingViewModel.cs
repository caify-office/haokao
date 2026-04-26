using HaoKao.CourseRatingService.Domain.Enums;
using HaoKao.CourseRatingService.Domain.Queries;

namespace HaoKao.CourseRatingService.Application.ViewModels;

[AutoMapFrom(typeof(CourseRatingQuery))]
[AutoMapTo(typeof(CourseRatingQuery))]
public class QueryCourseRatingViewModel : QueryDtoBase<QueryCourseRatingListViewModel>
{
    /// <summary>
    /// 课程Id
    /// </summary>
    [JsonIgnore]
    public string CourseId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonIgnore]
    public string NickName { get; set; }

    /// <summary>
    /// 课程名称
    /// </summary>
    [JsonIgnore]
    public string CourseName { get; set; }

    /// <summary>
    /// 评价开始时间
    /// </summary>
    [JsonIgnore]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 评价结束时间
    /// </summary>
    [JsonIgnore]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 评价级别
    /// </summary>
    [JsonIgnore]
    public int? Rating { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    [JsonIgnore]
    public AuditState? AuditState { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.CourseRating))]
[AutoMapTo(typeof(Domain.Entities.CourseRating))]
public record QueryCourseRatingListViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

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
    /// 审核状态
    /// </summary>
    public AuditState AuditState { get; init; }

    /// <summary>
    /// 评价人Id
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool Sticky { get; init; }
}