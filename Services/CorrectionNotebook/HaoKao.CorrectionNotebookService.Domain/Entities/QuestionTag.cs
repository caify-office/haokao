namespace HaoKao.CorrectionNotebookService.Domain.Entities;

/// <summary>
/// 题目标签关联实体类
/// </summary>
public sealed class QuestionTag : Entity
{
    /// <summary>
    /// 题目Id
    /// </summary>
    public Guid QuestionId { get; init; }

    /// <summary>
    /// 标签Id
    /// </summary>
    public Guid TagId { get; init; }

    /// <summary>
    /// 标签
    /// </summary>
    public Tag Tag { get; init; }
}