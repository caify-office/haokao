using HaoKao.FeedBackService.Application.ViewModels.FeedBackReply;
using HaoKao.FeedBackService.Domain.Enums;

namespace HaoKao.FeedBackService.Application.ViewModels.FeedBack;

[AutoMapFrom(typeof(Domain.Entities.FeedBack))]
public class BrowseFeedBackViewModel : IDto
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    public TypeEnum Type { get; set; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    public SourceTypeEnum SourceType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    public string Contract { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 图片
    /// </summary>
    public string FileUrls { get; set; }

    //回复列表

    public virtual List<BrowseFeedBackReplyViewModel> FeedBackReplies { get; set; }
    //追问

    [ForeignKey("ParentId")]
    public List<BrowseFeedBackViewModel> ChildQuestion { get; set; }
}