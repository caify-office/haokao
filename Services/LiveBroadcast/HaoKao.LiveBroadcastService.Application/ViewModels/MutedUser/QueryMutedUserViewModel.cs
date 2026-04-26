namespace HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;

[AutoMapFrom(typeof(MutedUserQuery))]
[AutoMapTo(typeof(MutedUserQuery))]
public class QueryMutedUserViewModel : QueryDtoBase<BrowseMutedUserViewModel>
{
    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
}