using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveCouponQuery : QueryBase<LiveCoupon>
{
    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    [QueryCacheKey]
    public Guid? LiveVideoId { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    [QueryCacheKey]
    public bool? IsShelves { get; set; }
    public override Expression<Func<LiveCoupon, bool>> GetQueryWhere()
    {
        Expression<Func<LiveCoupon, bool>> expression = x => true;
        if (LiveVideoId.HasValue)
        {
            expression = expression.And(x => x.LiveVideoId == LiveVideoId);
        }
        if (IsShelves.HasValue)
        {
            expression = expression.And(x => x.IsShelves == IsShelves);
        }
        return expression;
    }
}
