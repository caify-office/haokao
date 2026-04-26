using HaoKao.CourseRatingService.Domain.Enums;

namespace HaoKao.CourseRatingService.Domain.Entities;

/// <summary>
/// 课程评价
/// </summary>
[Comment("课程评价")]
public class CourseRating : AggregateRoot<Guid>,
                            ITenantShardingTable,
                            IIncludeMultiTenant<Guid>,
                            IIncludeCreateTime,
                            IIncludeUpdateTime,
                            IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 课程Id
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 课程名称
    /// </summary>
    public string CourseName { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// 评价级别
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public AuditState AuditState { get; set; }

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 评价人
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool Sticky { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}