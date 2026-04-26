namespace HaoKao.LearningPlanService.Application.ViewModels.LearningTask;

[AutoMapFrom(typeof(LearningTaskQuery))]
[AutoMapTo(typeof(LearningTaskQuery))]
public class LearningTaskQueryViewModel : QueryDtoBase<LearningTaskQueryListViewModel>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 计划开始该任务的时间点
    /// </summary>
    public DateOnly? ScheduledTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid? CreatorId { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.LearningTask))]
[AutoMapTo(typeof(Domain.Entities.LearningTask))]
public class LearningTaskQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    /// 计划开始该任务的时间点
    /// </summary>
    public DateOnly ScheduledTime { get; set; }

    /// <summary>
    /// 完成该任务预计需要的时长（秒），支持小数表示
    /// </summary>
    public decimal DurationSeconds { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public TaskType TaskType { get; set; }

    /// <summary>
    /// 任务标识（用于重新制作学习计划时判定当前任务内容是否已是过期任务内容）
    /// </summary>
    public string TaskIdentity { get; set; }

    /// <summary>
    /// 任务内容（存视频任务，章节任务任务，试卷任务详情）
    /// </summary>
    public string TaskContent { get; set; }

    /// <summary>
    /// 关联的学习计划ID
    /// </summary>
    public Guid LearningPlanId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}