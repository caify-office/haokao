namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;

[AutoMapFrom(typeof(LiveOnlineUserQuery))]
[AutoMapTo(typeof(LiveOnlineUserQuery))]
public class QueryLiveOnlineUserViewModel : QueryDtoBase<BrowseLiveOnlineUserViewModel>
{
    /// <summary>
    /// 直播Id
    /// </summary>
    [Required]
    public Guid LiveId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}