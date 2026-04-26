namespace HaoKao.BurialPointService.Application.ViewModels.BrowseRecord;


[AutoMapFrom(typeof(BrowseRecordQuery))]
[AutoMapTo(typeof(BrowseRecordQuery))]
public class BrowseRecordQueryViewModel : QueryDtoBase<BrowseRecordQueryListViewModel>
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }


    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 埋点id
    /// </summary>
    public Guid? BurialPointId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.BrowseRecord))]
[AutoMapTo(typeof(Domain.Entities.BrowseRecord))]
public class BrowseRecordQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }
    /// <summary>
    /// 埋点id
    /// </summary>
    public Guid BurialPointId { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 浏览信息
    /// </summary>
    public string BrowseData { get; set; }

    /// <summary>
    /// 是否付费用户
    /// </summary>
    public bool IsPaidUser { get; set; }
}