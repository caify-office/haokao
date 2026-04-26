namespace HaoKao.LearningPlanService.Application;

public interface ISortableTask
{
    /// <summary>
    /// 任务排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public TaskType TaskType { get; set; }

    /// <summary>
    /// 完成该任务预计需要的时长（秒），支持小数表示
    /// </summary>
    public decimal DurationSeconds { get; set; }
}