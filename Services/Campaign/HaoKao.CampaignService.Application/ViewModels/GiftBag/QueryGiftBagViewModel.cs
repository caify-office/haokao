using HaoKao.CampaignService.Domain.Enums;
using HaoKao.CampaignService.Domain.Queries;

namespace HaoKao.CampaignService.Application.ViewModels.GiftBag;

[AutoMapTo(typeof(GiftBagQuery))]
[AutoMapFrom(typeof(GiftBagQuery))]
public class QueryGiftBagViewModel : QueryDtoBase<BrowseGiftBagViewModel>
{
    /// <summary>
    /// 活动名称
    /// </summary>
    public string CampaignName { get; set; }

    /// <summary>
    /// 礼包类型
    /// </summary>
    public GiftType? GiftType { get; set; }
}