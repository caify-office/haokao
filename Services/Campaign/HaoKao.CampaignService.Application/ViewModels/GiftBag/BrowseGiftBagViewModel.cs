using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Enums;

namespace HaoKao.CampaignService.Application.ViewModels.GiftBag;

[AutoMapTo(typeof(Domain.Entities.GiftBag))]
[AutoMapFrom(typeof(Domain.Entities.GiftBag))]
public record BrowseGiftBagViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 活动名称
    /// </summary>
    public string CampaignName { get; init; }

    /// <summary>
    /// 礼包类型
    /// </summary>
    public GiftType GiftType { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublished { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; init; }

    /// <summary>
    /// 领取人数
    /// </summary>
    public int ReceiveCount { get; init; }

    /// <summary>
    /// PC网站图片
    /// </summary>
    public GiftBagImageSet WebSiteImageSet { get; init; }

    /// <summary>
    /// 微信小程序图片
    /// </summary>
    public GiftBagImageSet WeChatMiniProgramImageSet { get; init; }

    /// <summary>
    /// 领取规则
    /// </summary>
    public IReadOnlyList<Guid> ReceiveRules { get; init; }
}