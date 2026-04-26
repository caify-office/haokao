using HaoKao.CampaignService.Domain.Commands;
using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Enums;

namespace HaoKao.CampaignService.Application.ViewModels.GiftBag;

[AutoMapTo(typeof(CreateGiftBagCommand))]
public record CreateGiftBagViewModel : IDto
{
    /// <summary>
    /// 活动名称
    /// </summary>
    [DisplayName("活动名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string CampaignName { get; init; }

    /// <summary>
    /// 礼包类型
    /// </summary>
    [DisplayName("礼包类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public GiftType GiftType { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductName { get; init; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [DisplayName("开始时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [DisplayName("结束时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [DisplayName("是否发布")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsPublished { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; init; }

    /// <summary>
    /// PC网站图片
    /// </summary>
    [DisplayName("PC网站图片")]
    [Required(ErrorMessage = "{0}不能为空")]
    public GiftBagImageSet WebSiteImageSet { get; init; }

    /// <summary>
    /// 微信小程序图片
    /// </summary>
    [DisplayName("微信小程序图片")]
    [Required(ErrorMessage = "{0}不能为空")]
    public GiftBagImageSet WeChatMiniProgramImageSet { get; init; }

    /// <summary>
    /// 礼品包领取记录
    /// </summary>
    [DisplayName("礼品包领取记录")]
    [Required(ErrorMessage = "{0}不能为空")]
    public IReadOnlyList<Guid> ReceiveRules { get; init; }
}