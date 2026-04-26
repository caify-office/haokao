using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Enums;

namespace HaoKao.CampaignService.Domain.Queries;

public class GiftBagQuery : QueryBase<GiftBag>
{
    /// <summary>
    /// 活动名称
    /// </summary>
    [QueryCacheKey]
    public string CampaignName { get; init; }

    /// <summary>
    /// 礼包类型
    /// </summary>
    [QueryCacheKey]
    public GiftType? GiftType { get; init; }

    public override Expression<Func<GiftBag, bool>> GetQueryWhere()
    {
        Expression<Func<GiftBag, bool>> expression = x => true;

        if (!string.IsNullOrWhiteSpace(CampaignName))
        {
            expression = expression.And(x => x.CampaignName.Contains(CampaignName));
        }

        if (GiftType.HasValue)
        {
            expression = expression.And(x => x.GiftType == GiftType);
        }

        return expression;
    }
}