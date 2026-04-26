namespace HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;

[AutoMapFrom(typeof(Domain.Entities.MutedUser))]
public class BrowseMutedUserViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 禁言时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}