namespace HaoKao.LearningPlanService.Domain.Records;

/// <summary>
/// 试卷任务，对应完整的测试或考试
/// </summary>
public record PaperTaskContent
{
    /// <summary>
    /// 试卷ID
    /// </summary>
    public Guid PaperId { get; set; }

    /// <summary>
    /// 试卷名称
    /// </summary>
    public string PaperName { get; set; }
}