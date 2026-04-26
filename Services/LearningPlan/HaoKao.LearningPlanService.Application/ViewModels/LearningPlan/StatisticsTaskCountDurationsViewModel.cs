namespace HaoKao.LearningPlanService.Application.ViewModels.LearningPlan;

public class StatisticsTaskCountDurationsViewModel : IDto
{
    /// <summary>
    /// 任务数量
    /// </summary>
    public int TaskCount { get; set; }

    /// <summary>
    /// 任务总时长(分钟)
    /// </summary>
    public int TaskTotalDurations { get; set; }
}