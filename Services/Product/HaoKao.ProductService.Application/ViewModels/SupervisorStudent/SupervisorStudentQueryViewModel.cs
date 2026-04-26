using HaoKao.ProductService.Domain.Queries;

namespace HaoKao.ProductService.Application.ViewModels.SupervisorStudent;


[AutoMapFrom(typeof(SupervisorStudentQuery))]
[AutoMapTo(typeof(SupervisorStudentQuery))]
public class SupervisorStudentQueryViewModel : QueryDtoBase<SupervisorStudentQueryListViewModel>
{
    /// <summary>
    /// 督学班级id
    /// </summary>
    [Required]
    public Guid SupervisorClassId { get; set; }
    /// <summary>
    /// 手机号/用户名称
    /// </summary>
    public string PhoneOrNickName { get; set; }

    /// <summary>
    /// 上次学习开始时间最小值
    /// </summary>
    public DateTime? MinLearnTime { get; set; }

    /// <summary>
    /// 上次学习结束时间最大值
    /// </summary>
    public DateTime? MaxLearnTime { get; set; }

    /// <summary>
    /// 距上次时隔天数最小值
    /// </summary>
    public int? MinLearnIntervalDays { get; set; }

    /// <summary>
    /// 距上次时隔天数最大值
    /// </summary>
    public int? MaxLearnIntervalDays { get; set; }

    /// <summary>
    /// 最小课程学习进度
    /// </summary>
    public float? MinCourseRatio { get; set; }

    /// <summary>
    /// 最大课程学习进度
    /// </summary>
    public float? MaxCourseRatio { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.SupervisorStudent))]
[AutoMapTo(typeof(Domain.Entities.SupervisorStudent))]
public class SupervisorStudentQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 督学班级id
    /// </summary>
    public Guid SupervisorClassId { get; set; }

    /// <summary>
    /// 注册用户id
    /// </summary>
    public Guid RegisterUserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 最大听课时长
    /// </summary>
    public float MaxProgress { get; set; }

    /// <summary>
    /// 总时长
    /// </summary>
    public float CourseDuration { get; set; }
    /// <summary>
    /// 总学习进度
    /// </summary>
    public float CourseRatio { get; set; }
    /// <summary>
    /// 已学完课程数
    /// </summary>
    public int IsEndCourseCount { get; set; }

    /// <summary>
    /// 课程数
    /// </summary>
    public int CourseCount { get; set; }

    /// <summary>
    /// 最近学习时间
    /// </summary>
    public DateTime? LastLearnTime { get; set; }

    /// <summary>
    /// 最近学习间隔天数
    /// </summary>
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