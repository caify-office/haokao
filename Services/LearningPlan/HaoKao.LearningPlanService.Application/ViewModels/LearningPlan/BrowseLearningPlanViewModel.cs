namespace HaoKao.LearningPlanService.Application.ViewModels.LearningPlan;

[AutoMapFrom(typeof(Domain.Entities.LearningPlan))]
public class BrowseLearningPlanViewModel : IDto
{
    public Guid Id { get; set; }

    public DateTime ServiceTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 学习计划结束日期
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// 该学习计划包含的所有任务
    /// </summary>
    public List<Domain.Entities.LearningTask> LearningTasks { get; set; }

    /// <summary>
    /// 每周学习时长配置(分钟)
    /// </summary>
    public List<int> DayLearningTimes { get; set; }

    /// <summary>
    /// 需要提醒
    /// </summary>
    public bool NeedReminder { get; set; }

    /// <summary>
    /// 提醒时间(小时)
    /// </summary>
    public int ReminderHours { get; set; }

    /// <summary>
    /// 提醒时间(分钟)
    /// </summary>
    public int ReminderMinutes { get; set; }

    /// <summary>
    /// 提醒手机号
    /// </summary>
    public string ReminderPhone { get; set; }
}