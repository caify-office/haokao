using System.ComponentModel.DataAnnotations.Schema;
using HaoKao.DataStatisticsService.WebApi.Enums;

namespace HaoKao.DataStatisticsService.WebApi.Models;

/// <summary>
/// 学习情况
/// </summary>
public record ProgressStatistics : IIncludeMultiTenant<Guid>, Entity
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid RegisterUserId { get; init; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; init; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; init; }

    /// <summary>
    /// 是否付费学员
    /// </summary>
    public bool IsPaidStudent { get; init; }

    /// <summary>
    ///  最大听课时长
    /// </summary>
    public float MaxProgress { get; init; }

    /// <summary>
    /// 总时长
    /// </summary>
    public float CourseDuration { get; init; }

    /// <summary>
    /// 总学习进度
    /// </summary>
    public float CourseRatio { get; init; }


    /// <summary>
    /// 课程数
    /// </summary>
    public int CourseCount { get; init; }

    /// <summary>
    /// 已学完课程数
    /// </summary>
    public int IsEndCourseCount { get; init; }

    /// <summary>
    /// 最近学习课程时间
    /// </summary>
    public DateTime? LastLearnCourseTime { get; init; }

    /// <summary>
    /// 课程权限状态
    /// </summary>
    public PermissionExpiryType? PermissionExpiryType1 { get; init; }

    /// <summary>
    /// 题库权限状态
    /// </summary>
    public PermissionExpiryType? PermissionExpiryType2 { get; init; }

    /// <summary>
    /// 试题总数
    /// </summary>
    public int QuestionCount { get; init; }

    /// <summary>
    /// 已答题数
    /// </summary>
    public int AnsweredCount { get; init; }

    /// <summary>
    /// 答题进度
    /// </summary>
    public float AnswerRadio { get; init; }

    /// <summary>
    /// 最近答题时间
    /// </summary>
    public DateTime? LastAnswerTime { get; init; }

    /// <summary>
    /// 最近学习时间
    /// </summary>
    public DateTime? LastLearnTime { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 最近学习间隔天数
    /// </summary>
    [NotMapped]
    public int LastLearnIntervalDays
    {
        get
        {
            if (LastLearnTime.HasValue)
            {
                return (DateTime.Now - LastLearnTime.Value).Days;
            }
            else
            {
                return -1;
            }
        }
    }
}
