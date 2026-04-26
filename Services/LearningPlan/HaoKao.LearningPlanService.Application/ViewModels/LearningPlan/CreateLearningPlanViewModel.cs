using HaoKao.Common.RemoteModel;
using HaoKao.LearningPlanService.Domain.Records;

namespace HaoKao.LearningPlanService.Application.ViewModels.LearningPlan;

[AutoMapTo(typeof(CreateLearningPlanCommand))]
public class CreateLearningPlanViewModel : IDto
{
    /// <summary>
    /// id有值则更新之前的学习计划，无值则创建新的学习计划
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    public string ProductName { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    [DisplayName("对应的科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    [DisplayName("对应科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string SubjectName { get; set; }

    /// <summary>
    /// 学习计划结束日期
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// 每周学习时长配置
    /// </summary>
    public List<int> DayLearningTimes { get; set; }

    /// <summary>
    /// 需要提醒
    /// </summary>
    public bool NeedReminder { get; set; }

    /// <summary>
    /// 提醒时间(小时)
    /// </summary>
    public int ReminderHour { get; set; }

    /// <summary>
    /// 提醒时间(分钟)
    /// </summary>
    public int ReminderMinute { get; set; }

    /// <summary>
    /// 提醒手机号
    /// </summary>
    public string ReminderPhone { get; set; }

    /// <summary>
    /// 选择的知识点任务
    /// </summary>
    public virtual ICollection<KnowledgePointTask> KnowledgePointTasks { get; set; } = new List<KnowledgePointTask>();

    /// <summary>
    /// 系统配置的阶段任务
    /// </summary>
    public virtual ICollection<AssistantProductPermissionContent> StateTasks { get; set; } = new List<AssistantProductPermissionContent>();
}