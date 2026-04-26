using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LivePlayBackQuery : QueryBase<LivePlayBack>
{
    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    [QueryCacheKey]
    public Guid? LiveVideoId { get; set; }

    public override Expression<Func<LivePlayBack, bool>> GetQueryWhere()
    {
        Expression<Func<LivePlayBack, bool>> expression = x => true;
        if (LiveVideoId.HasValue)
        {
            expression = expression.And(x => x.LiveVideoId == LiveVideoId);
        }
        return expression;
    }
}