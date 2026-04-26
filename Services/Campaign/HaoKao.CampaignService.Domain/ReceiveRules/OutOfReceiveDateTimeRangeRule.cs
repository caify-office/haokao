using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Domain.ReceiveRules;

public class OutOfReceiveDateTimeRangeRule : IReceiveRule
{
    public Guid RuleId => new("4e301af6-cd3a-47e8-9903-8ba0eb04dd9f");

    public string RuleName => "领取时间范围";

    public string BrokenMessage => "超过领取时间范围";

    public bool Internal => true;

    private GiftBag _giftBag;

    public bool IsBroken(IReceiveRuleParam receiveRuleParam)
    {
        _giftBag = receiveRuleParam.GiftBag;
        return _giftBag.StartTime >= DateTime.Now || _giftBag.EndTime <= DateTime.Now;
    }
}