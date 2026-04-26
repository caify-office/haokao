namespace HaoKao.CorrectionNotebookService.Domain.Entities;

/// <summary>
/// 题目答案和解析
/// </summary>
public sealed class GenerationLog : AggregateRoot<Guid>,
                                     IIncludeCreatorId<Guid>,
                                     IIncludeCreatorName,
                                     IIncludeCreateTime
{
    /// <summary>
    /// 题目Id
    /// </summary>
    public Guid QuestionId { get; init; }

    /// <summary>
    /// 题目答案
    /// </summary>
    public string Answer { get; init; }

    /// <summary>
    /// 题目解析
    /// </summary>
    public string Analysis { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}