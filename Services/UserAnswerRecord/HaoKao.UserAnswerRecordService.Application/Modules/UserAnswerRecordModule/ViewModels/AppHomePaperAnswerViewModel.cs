namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

public class AppHomePaperAnswerViewModel
{
    /// <summary>
    /// 试卷Id
    /// </summary>
    public Guid PaperId { get; set; }

    /// <summary>
    /// 参与考试人数
    /// </summary>
    public int ExamNumber { get; set; }

    /// <summary>
    /// 当前的进度
    /// </summary>
    public int? Progress { get; set; }

    /// <summary>
    /// 当前自己得分
    /// </summary>
    public decimal? CurrentScore { get; set; }
}