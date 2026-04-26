namespace HaoKao.LearningPlanService.Domain.Enumerations;

/// <summary>
/// 任务类型
/// </summary>
public enum TaskType
{
    /// <summary>
    /// 视频任务
    /// </summary>
    VideoTask = 0,

    /// <summary>
    /// 章节练习任务
    /// </summary>
    ExerciseTask = 1,

    /// <summary>
    /// 考试任务
    /// </summary>
    PaperTask = 2,
}