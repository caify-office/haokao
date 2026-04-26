namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveComment;

[AutoMapFrom(typeof(LiveCommentQuery))]
[AutoMapTo(typeof(LiveCommentQuery))]
public class QueryLiveCommentViewModel : QueryDtoBase<BrowseLiveCommentViewModel>
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

    /// <summary>
    /// 评分
    /// </summary>
    public int? Rating { get; set; }
}