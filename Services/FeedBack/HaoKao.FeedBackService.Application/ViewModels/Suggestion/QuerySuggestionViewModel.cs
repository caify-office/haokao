using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Queries.EntityQuery;
using Newtonsoft.Json;

namespace HaoKao.FeedBackService.Application.ViewModels.Suggestion;

[AutoMapFrom(typeof(SuggestionQuery))]
[AutoMapTo(typeof(SuggestionQuery))]
public class QuerySuggestionViewModel : QueryDtoBase<BrowseSuggestionViewModel>
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    public SuggestionType? SuggestionType { get; set; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    public string SuggestionFrom { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 反馈人名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 开始反馈时间
    /// </summary>
    public DateTime? StartCreateTime { get; set; }

    /// <summary>
    /// 结束反馈时间
    /// </summary>
    public DateTime? EndCreateTime { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    public ReplyState? ReplyState { get; set; }

    /// <summary>
    /// 处理人名称
    /// </summary>
    public string ReplyUserName { get; set; }

    /// <summary>
    /// 开始回复时间
    /// </summary>
    public DateTime? StartReplyTime { get; set; }

    /// <summary>
    /// 结束回复时间
    /// </summary>
    public DateTime? EndReplyTime { get; set; }

    /// <summary>
    /// 反馈人Id
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore, JsonIgnore]
    public Guid? CreatorId { get; set; }
}