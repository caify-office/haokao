using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Domain.Queries;

public class GiftBagPublishedByUserQuery(Guid? userId)
{
    public Expression<Func<GiftBag, bool>> Criteria => x => x.IsPublished && x.StartTime <= DateTime.Now && DateTime.Now <= x.EndTime;

    public Expression<Func<GiftBag, object>> Include => x => x.GiftBagReceiveLogs;

    public Expression<Func<GiftBag, int>> OrderBy => x => x.Sort;

    public Expression<Func<GiftBag, GiftBag>> Selector => x => new GiftBag
    {
        Id = x.Id,
        CampaignName = x.CampaignName,
        GiftType = x.GiftType,
        ProductId = x.ProductId,
        ProductName = x.ProductName,
        StartTime = x.StartTime,
        EndTime = x.EndTime,
        IsPublished = x.IsPublished,
        Sort = x.Sort,
        ReceiveCount = x.ReceiveCount,
        WebSiteImageSet = x.WebSiteImageSet,
        WeChatMiniProgramImageSet = x.WeChatMiniProgramImageSet,
        GiftBagReceiveLogs = userId.HasValue ? x.GiftBagReceiveLogs.Where(y => y.ReceiverId == userId).ToList() : new(0),
        ReceiveRules = x.ReceiveRules,
        CreateTime = x.CreateTime,
        UpdateTime = x.UpdateTime,
        TenantId = x.TenantId,
    };
}