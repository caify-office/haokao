namespace HaoKao.CorrectionNotebookService.Domain.ValueObjects;

/// <summary>
/// 用户题目统计
/// </summary>
/// <param name="TotalCount">总题数</param>
/// <param name="MasteredCount">已掌握</param>
/// <param name="RestOfCount">剩余题数</param>
public record UserQuestionCountStatistics(int TotalCount, int MasteredCount, int RestOfCount);