


namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;

public class LearnRcordByWeekStaticViewModel : IDto
{
    /// <summary>
    /// 日期
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// 进度--注意这里是总的进度
    /// </summary>
    public float Progress { get; set; }
}