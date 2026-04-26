namespace HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;

/// <summary>
/// 考生答题内容
/// </summary>
public class UserAnswerContent
{
    /// <summary>
    /// 对应选项或填空相关联的Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 作答内容
    /// </summary>
    public string Content { get; set; }
}