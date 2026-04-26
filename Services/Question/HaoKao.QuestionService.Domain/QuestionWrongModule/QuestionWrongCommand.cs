namespace HaoKao.QuestionService.Domain.QuestionWrongModule;

/// <summary>
/// 消灭错题
/// </summary>
/// <param name="QuestionIds">试题Ids</param>
public record CleanQuestionWrongCommand(List<Guid> QuestionIds) : Command("消灭错题");

/// <summary>
/// 添加错题集
/// </summary>
/// <param name="QuestionIds">试题Ids</param>
public record CreateQuestionWrongCommand(List<Guid> QuestionIds) : Command("添加错题集");