namespace HaoKao.LearnProgressService.Application.ViewModels.DailyStudyDuration;


public class QueryDailyLearnDataViewModel : IDto
{

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateOnly EndDate { get; init; }
}


public record DateStudyDurationViewModel
{
    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// 时长 (小时)
    /// </summary>
    public decimal Duration { get; init; }
}
