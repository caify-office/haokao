using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Domain.Queries;

public class GiftBagReceiveLogQuery : QueryBase<GiftBagReceiveLog>
{
    /// <summary>
    /// 礼品包Id
    /// </summary>
    [QueryCacheKey]
    public Guid GiftBagId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [QueryCacheKey]
    public string ReceiverName { get; set; }

    /// <summary>
    /// 领取时间-开始
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 领取时间-结束
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndTime { get; set; }

    public override Expression<Func<GiftBagReceiveLog, bool>> GetQueryWhere()
    {
        Expression<Func<GiftBagReceiveLog, bool>> expression = x => x.GiftBagId == GiftBagId;

        if (!string.IsNullOrEmpty(ReceiverName))
        {
            expression = expression.And(x => x.ReceiverName.Contains(ReceiverName));
        }

        if (StartTime.HasValue)
        {
            expression = expression.And(x => x.ReceiveTime >= StartTime);
        }

        if (EndTime.HasValue)
        {
            expression = expression.And(x => x.ReceiveTime <= EndTime);
        }

        return expression;
    }
}