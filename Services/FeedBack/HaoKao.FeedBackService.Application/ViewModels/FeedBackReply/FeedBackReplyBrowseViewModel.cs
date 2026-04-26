namespace HaoKao.FeedBackService.Application.ViewModels.FeedBackReply;

[AutoMapFrom(typeof(Domain.Entities.FeedBackReply))]
public class BrowseFeedBackReplyViewModel : IDto
{
    /// <summary>
    /// 答疑回复内容
    /// </summary>
    public string ReplyContent { get; set; }

    /// <summary>
    /// 回复人用户名
    /// </summary>
    public string ReplyUserName { get; set; }

    /// <summary>
    /// 关联的题目id
    /// </summary>
    public Guid FeedBackId { get; set; }

    /// <summary>
    /// 文件地址
    /// </summary>
    public string FileUrl { get; set; }
}