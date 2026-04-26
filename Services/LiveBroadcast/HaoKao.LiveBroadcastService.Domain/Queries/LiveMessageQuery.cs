using HaoKao.LiveBroadcastService.Domain.Entities;
using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveMessageQuery : QueryBase<LiveMessage>
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [QueryCacheKey]
    public LiveMessageType? MessageType { get; set; }

    /// <summary>
    /// 直播间Id
    /// </summary>
    [QueryCacheKey]
    public Guid? LiveId { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? SendTime { get; set; }

    public override Expression<Func<LiveMessage, bool>> GetQueryWhere()
    {
        Expression<Func<LiveMessage, bool>> expression = x => true;

        if (MessageType.HasValue)
        {
            expression = expression.And(x => x.LiveMessageType == MessageType.Value);
        }
        if (LiveId.HasValue)
        {
            expression = expression.And(x => x.LiveId == LiveId.Value);
        }
        if (SendTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime < SendTime.Value);
        }

        return expression;
    }
}