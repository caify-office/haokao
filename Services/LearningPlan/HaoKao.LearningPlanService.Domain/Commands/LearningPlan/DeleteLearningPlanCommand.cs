namespace HaoKao.LearningPlanService.Domain.Commands.LearningPlan;

/// <summary>
/// 删除学习计划主类，用于组织和管理一系列学习任务命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLearningPlanCommand(Guid Id) : Command("删除学习计划主类，用于组织和管理一系列学习任务");