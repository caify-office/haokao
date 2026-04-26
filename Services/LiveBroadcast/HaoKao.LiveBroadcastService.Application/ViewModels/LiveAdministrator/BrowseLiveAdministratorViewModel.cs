namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveAdministrator;

[AutoMapFrom(typeof(Domain.Entities.LiveAdministrator))]
public class BrowseLiveAdministratorViewModel : IDto
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}