namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveAdministrator;

[AutoMapFrom(typeof(LiveAdministratorQuery))]
[AutoMapTo(typeof(LiveAdministratorQuery))]
public class LiveAdministratorQueryViewModel : QueryDtoBase<LiveAdministratorQueryListViewModel>
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.LiveAdministrator))]
[AutoMapTo(typeof(Domain.Entities.LiveAdministrator))]
public class LiveAdministratorQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}