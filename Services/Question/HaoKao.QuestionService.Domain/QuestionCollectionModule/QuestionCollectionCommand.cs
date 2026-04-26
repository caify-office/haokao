namespace HaoKao.QuestionService.Domain.QuestionCollectionModule;

/// <summary>
/// 收藏试题
/// </summary>
/// <param name="QuestionId">试题Id</param>
public record CreateQuestionCollectionCommand(Guid QuestionId) : Command("收藏试题");

/// <summary>
/// 取消收藏试题
/// </summary>
/// <param name="QuestionId">试题Id</param>
public record DeleteQuestionCollectionCommand(Guid QuestionId) : Command("取消收藏试题");