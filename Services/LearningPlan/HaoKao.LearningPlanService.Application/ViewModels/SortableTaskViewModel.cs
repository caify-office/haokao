namespace HaoKao.LearningPlanService.Application.ViewModels;

public class SortableTaskViewModel
{
    /// <summary>
    /// 完成该任务预计需要的时长（秒），支持小数表示
    /// </summary>
    public decimal DurationSeconds { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public TaskType TaskType { get; set; }

    /// <summary>
    /// 任务身份认证,用于除去已排并且已过期的任务（视频任务为VideoId,章节练习任务为ChapterId,试卷任务为PaperId）
    /// </summary>
    public Guid TaskIdentity { get; set; }

    /// <summary>
    /// 任务内容(可根据任务类型转为具体的任务)
    /// </summary>
    public string TaskContent { get; set; }
}