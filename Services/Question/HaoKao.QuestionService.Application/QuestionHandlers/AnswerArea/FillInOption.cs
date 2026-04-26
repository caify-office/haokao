namespace HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;

/// <summary>
/// 填空题项
/// </summary>
public class FillInOption : AnswerAreaBase
{
    /// <summary>
    /// 参考答案
    /// </summary>
    public string AnswerContent { get; set; }
}