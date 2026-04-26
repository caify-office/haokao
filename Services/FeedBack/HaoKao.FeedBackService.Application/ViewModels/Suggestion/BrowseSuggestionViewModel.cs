using HaoKao.FeedBackService.Domain.Entities;

namespace HaoKao.FeedBackService.Application.ViewModels.Suggestion;

[AutoMapFrom(typeof(Domain.Entities.Suggestion))]
public class BrowseSuggestionViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 反馈类型
    /// </summary>
    public SuggestionType SuggestionType { get; set; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    public string SuggestionFrom { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 问题描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 相关截图
    /// </summary>
    public List<string> Screenshots { get; set; }

    /// <summary>
    /// 反馈人Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 反馈人名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 反馈时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    public int ReplyState { get; set; }

    /// <summary>
    /// 处理人Id
    /// </summary>
    public Guid? ReplyUserId { get; set; }

    /// <summary>
    /// 处理人名称
    /// </summary>
    public string ReplyUserName { get; set; }

    /// <summary>
    /// 回复时间
    /// </summary>
    public DateTime? ReplyTime { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    public string ReplyContent { get; set; }

    /// <summary>
    /// 回复截图
    /// </summary>
    public List<string> ReplyScreenshots { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}