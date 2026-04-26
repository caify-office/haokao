using HaoKao.Common.RemoteModel;
using HaoKao.LearningPlanService.Domain.Records;

namespace HaoKao.LearningPlanService.Domain.Commands.LearningPlan;

/// <summary>
/// 创建学习计划主类，用于组织和管理一系列学习任务命令
/// </summary>
/// <param name="Id"></param>
/// <param name="ProductId">产品Id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="SubjectId">对应的科目Id</param>
/// <param name="SubjectName">对应科目名称</param>
/// <param name="EndDate">学习计划结束日期</param>
/// <param name="DayLearningTimes">每周学习时长配置(分钟)</param>
/// <param name="NeedReminder">需要提醒</param>
/// <param name="ReminderHours">提醒时间(小时)</param>
/// <param name="ReminderMinutes">提醒时间(分钟)</param>
/// <param name="ReminderPhone">提醒的手机号</param>
/// <param name="KnowledgePointTasks">选择的知识点任务</param>
/// <param name="StateTasks">系统配置的阶段任务</param>
public record CreateLearningPlanCommand(
    Guid? Id = null,
    Guid ProductId = default,
    string ProductName = null,
    Guid SubjectId = default,
    string SubjectName = "",
    DateOnly EndDate = default,
    List<int> DayLearningTimes = null,
    bool NeedReminder = false,
    int ReminderHours = 0,
    int ReminderMinutes = 0,
    string ReminderPhone = "",
    ICollection<KnowledgePointTask> KnowledgePointTasks = null,
    ICollection<AssistantProductPermissionContent> StateTasks = null
) : Command("创建/修改学习计划主类，用于组织和管理一系列学习任务")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => SubjectName)
                 .NotEmpty().WithMessage("对应科目名称不能为空")
                 .MaximumLength(50).WithMessage("对应科目名称长度不能大于50")
                 .MinimumLength(2).WithMessage("对应科目名称长度不能小于2");
    }
}