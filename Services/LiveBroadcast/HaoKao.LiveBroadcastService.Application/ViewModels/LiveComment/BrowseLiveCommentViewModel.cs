namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveComment;

[AutoMapFrom(typeof(Domain.Entities.LiveComment))]
public class BrowseLiveCommentViewModel : IDto
{
    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 评分
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string CreatorName { get; set; }
}