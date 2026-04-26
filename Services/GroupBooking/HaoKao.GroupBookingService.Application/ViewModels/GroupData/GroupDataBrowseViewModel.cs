namespace HaoKao.GroupBookingService.Application.ViewModels.GroupData;

[AutoMapFrom(typeof(Domain.Entities.GroupData))]
public class BrowseGroupDataViewModel : IDto
{
    /// <summary>
    /// 团名
    /// </summary>
    public string DataName { get; set; }

    /// <summary>
    /// 适用科目
    /// </summary>
    public List<Guid> SuitableSubjects { get; set; }

    /// <summary>
    /// 成团人数
    /// </summary>
    public int PeopleNumber { get; set; }

    /// <summary>
    /// 限制时间
    /// </summary>
    public int LimitTime { get; set; }

    /// <summary>
    /// 拼团资料
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool State { get; set; }
}