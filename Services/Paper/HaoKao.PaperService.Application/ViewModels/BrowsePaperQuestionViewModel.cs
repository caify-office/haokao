namespace HaoKao.PaperService.Application.ViewModels;

public class BrowsePaperQuestionInfoViewModel
{
    /// <summary>
    /// 题型名称
    /// </summary>
    public string Typename { get; set; }

    /// <summary>
    /// 题型Id
    /// </summary>
    public string TypeId { get; set; }

    /// <summary>
    /// 题目总数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 每题分数
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 判题规则
    /// </summary>
    public ScoringRule ScoringRules { get; set; }
}

public class BrowsePaperQuestionViewModel : BrowsePaperQuestionInfoViewModel
{
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 试题
    /// </summary>
    public IReadOnlyList<QuestionInfo> Questions { get; set; }
}

public record BrowsePaperQuestionCountViewModel(int PaperCount, int PaperQuestionCount);