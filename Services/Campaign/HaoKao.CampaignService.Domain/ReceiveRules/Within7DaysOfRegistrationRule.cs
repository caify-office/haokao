namespace HaoKao.CampaignService.Domain.ReceiveRules;

/// <summary>
/// 注册7天日可领取
/// </summary>
public class Within7DaysOfRegistrationRule : IReceiveRule
{
    public Guid RuleId => new("3ee7f5bd-754a-41e4-9d09-0b013d48483b");

    public string RuleName => "注册7日内";

    public string BrokenMessage => "仅限注册7日内的新用户领取";

    public bool Internal => false;

    private DateTime _registrationTime;

    public bool IsBroken(IReceiveRuleParam receiveRuleParam)
    {
        _registrationTime = receiveRuleParam.RegistrationTime;
        return DateTime.Now - _registrationTime > TimeSpan.FromDays(7);
    }
}