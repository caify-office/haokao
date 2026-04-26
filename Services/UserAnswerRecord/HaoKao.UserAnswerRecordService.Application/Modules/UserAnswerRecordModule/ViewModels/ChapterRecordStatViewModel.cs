namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

public class ChapterRecordStatViewModel
{
    /// <summary>
    /// 用户答题数
    /// </summary>
    public int AnswerCount { get; set; }

    /// <summary>
    /// 用户已练章节数
    /// </summary>
    public int ChapterCount { get; set; }

    /// <summary>
    /// 每题耗时
    /// </summary>
    public long QuestionTime { get; set; }

    /// <summary>
    /// 正确率
    /// </summary>
    public double Accuracy { get; set; }
}