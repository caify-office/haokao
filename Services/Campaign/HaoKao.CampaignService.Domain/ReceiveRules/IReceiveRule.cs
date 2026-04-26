using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Domain.ReceiveRules;

public interface IReceiveRule
{
    /// <summary>
    /// 规则Id
    /// </summary>
    Guid RuleId { get; }

    /// <summary>
    /// 规则名称
    /// </summary>
    string RuleName { get; }

    /// <summary>
    /// 违反规则的消息
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    string BrokenMessage { get; }

    /// <summary>
    /// 是否内置规则
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    bool Internal { get; }

    /// <summary>
    /// 是否违反领取规则
    /// </summary>
    /// <returns></returns>
    bool IsBroken(IReceiveRuleParam receiveRuleParam);
}

public interface IReceiveRuleParam
{
    /// <summary>
    /// 礼包
    /// </summary>
    GiftBag GiftBag { get; }

    /// <summary>
    /// 领取人Id
    /// </summary>
    Guid ReceiverId { get; }

    /// <summary>
    /// 注册时间
    /// </summary>
    DateTime RegistrationTime { get; }
}

public record ReceiveRuleParam : IReceiveRuleParam
{
    public ReceiveRuleParam(GiftBag giftBag, Guid receiverId, DateTime registrationTime)
    {
        GiftBag = giftBag;
        ReceiverId = receiverId;
        RegistrationTime = registrationTime;
    }

    public GiftBag GiftBag { get; }

    public Guid ReceiverId { get; }

    public DateTime RegistrationTime { get; }
}