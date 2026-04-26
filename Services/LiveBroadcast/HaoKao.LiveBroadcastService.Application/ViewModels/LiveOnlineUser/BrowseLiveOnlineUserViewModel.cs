namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;

[AutoMapFrom(typeof(Domain.Entities.LiveOnlineUser))]
[AutoMapTo(typeof(Domain.Entities.LiveOnlineUser))]
public class BrowseLiveOnlineUserViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 累计在线时长
    /// </summary>
    public int OnlineDuration { get; set; }

    /// <summary>
    /// 首次上线时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 最后上线时间
    /// </summary>
    public DateTime LastOnlineTime { get; set; }

    /// <summary>
    /// 系统时间
    /// </summary>
    public DateTime SystemTime { get; set; } = DateTime.Now;
}

public class OnlineUserViewModel(Guid userId, string userName) : IDto
{
    public Guid UserId { get; set; } = userId;

    public string UserName { get; set; } = userName;
}