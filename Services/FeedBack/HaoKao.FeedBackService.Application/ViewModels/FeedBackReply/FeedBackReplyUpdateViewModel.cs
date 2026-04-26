namespace HaoKao.FeedBackService.Application.ViewModels.FeedBackReply;

[AutoMapTo(typeof(Domain.Entities.FeedBackReply))]
public class UpdateFeedBackReplyViewModel : IDto
{
    /// <summary>
    /// 答疑回复内容
    /// </summary>
    [DisplayName("答疑回复内容")]
    [Required(ErrorMessage = "{0}不能为空")]

    public string ReplyContent { get; set; }

    /// <summary>
    /// 回复人用户名
    /// </summary>
    [DisplayName("回复人用户名")]
    [Required(ErrorMessage = "{0}不能为空")]

    public string ReplyUserName { get; set; }

    /// <summary>
    /// 关联的题目id
    /// </summary>
    [DisplayName("关联的题目id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid FeedBackId { get; set; }
}