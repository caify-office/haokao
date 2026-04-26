using HaoKao.FeedBackService.Application.ViewModels.FeedBackReply;
using HaoKao.FeedBackService.Domain.Enums;
using HaoKao.FeedBackService.Domain.Queries.EntityQuery;

namespace HaoKao.FeedBackService.Application.ViewModels.FeedBack;

[AutoMapFrom(typeof(FeedBackQuery))]
[AutoMapTo(typeof(FeedBackQuery))]
public class FeedBackQueryViewModel : QueryDtoBase<FeedBackQueryListViewModel>
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    [QueryCacheKey]
    public TypeEnum? Type { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    [QueryCacheKey]
    public SourceTypeEnum? SourceType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [QueryCacheKey]
    public StatusEnum? Status { get; set; }

    /// <summary>
    /// 反馈开始时间
    /// </summary>
    [QueryCacheKey]
    public string StartTime { get; set; }

    /// <summary>
    /// 反馈结束时间
    /// </summary>
    [QueryCacheKey]
    public string EndTime { get; set; }

    /// <summary>
    /// 处理人
    /// </summary>
    [QueryCacheKey]
    public string HanldUserName { get; set; }

    /// <summary>
    /// 处理人
    /// </summary>
    [QueryCacheKey]
    public string FeedBackUserName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.FeedBack))]
[AutoMapTo(typeof(Domain.Entities.FeedBack))]
public class FeedBackQueryListViewModel : IDto
{
    public Guid Id { get; set; }

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

    /// <summary>
    /// 反馈时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 反馈人
    /// </summary>
    public string CreatorName { get; set; }

    //回复列表
    public virtual List<BrowseFeedBackReplyViewModel> FeedBackReplies { get; set; }
    //追问

    [ForeignKey("ParentId")]
    public List<BrowseFeedBackViewModel> ChildQuestion { get; set; }
}