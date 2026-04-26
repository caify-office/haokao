namespace HaoKao.LearnProgressService.Application.ViewModels.DailyStudyDuration;


[AutoMapFrom(typeof(Domain.Entities.DailyStudyDuration))]
public class BrowseDailyStudyDurationViewModel : IDto
{

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId{ get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId{ get; set; }

    /// <summary>
    /// 今日视频学习时长(小时)
    /// </summary>
    public decimal DailyVideoStudyDuration { get; set; }

    /// <summary>
    /// 学习时间
    /// </summary>
    public DateOnly LearnTime{ get; set; }
}
