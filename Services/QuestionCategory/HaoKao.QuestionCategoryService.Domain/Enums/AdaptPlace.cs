namespace HaoKao.QuestionCategoryService.Domain.Enums;

/// <summary>
/// 适应场景
/// </summary>
public enum AdaptPlace : long
{
    /// <summary>
    /// 试题
    /// </summary>
    Question = 1,

    /// <summary>
    /// 试卷
    /// </summary>
    Paper = 2,

    /// <summary>
    /// 所有
    /// </summary>
    All = Question | Paper
}