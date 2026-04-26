namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;

public class LearnRcordStaticViewModel : IDto
{
    /// <summary>
    /// 已学习的进度统计--注意这里取得最大的学习进度
    /// </summary>
    public double LearnTotalTime { get; set; }

    /// <summary>
    /// 视频总的市场统计
    /// </summary>
    public double VideoTotalTime { get; set; }
}