namespace HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;

/// <summary>
/// 选择题选项
/// </summary>
public class ChoiceQuestionOption : AnswerAreaBase
{
    /// <summary>
    /// 选项文本
    /// </summary>
    public string OptionContent { get; set; }

    /// <summary>
    /// 是否为答案
    /// </summary>
    public bool IsAnswer { get; set; }
}