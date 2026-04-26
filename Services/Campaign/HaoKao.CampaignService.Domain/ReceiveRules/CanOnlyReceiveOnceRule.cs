using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Domain.ReceiveRules;

/// <summary>
/// 只能领取一次
/// </summary>
public class CanOnlyReceiveOnceRule : IReceiveRule
{
    public Guid RuleId => new("091629fa-8ac0-4d81-ac58-9e0dc943f90b");

    public string RuleName => "只能领取一次";

    public string BrokenMessage => "礼包每人只能领取一次";

    public bool Internal => true;

    private GiftBag _giftBag;

    private Guid _receiverId;

    public bool IsBroken(IReceiveRuleParam receiveRuleParam)
    {
        _giftBag = receiveRuleParam.GiftBag;
        _receiverId = receiveRuleParam.ReceiverId;
        return _giftBag.GiftBagReceiveLogs.Any(x => x.ReceiverId == _receiverId);
    }
}