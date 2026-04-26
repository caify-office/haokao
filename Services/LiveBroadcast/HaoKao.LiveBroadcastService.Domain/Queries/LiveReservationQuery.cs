using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveReservationQuery : QueryBase<LiveReservation>
{
    /// <summary>
    /// 直播Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 预约视频直播Id
    /// </summary>
    [QueryCacheKey]
    public Guid? LiveVideoId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [QueryCacheKey]
    public string CreatorName { get; set; }

    public override Expression<Func<LiveReservation, bool>> GetQueryWhere()
    {
        Expression<Func<LiveReservation, bool>> expression = x => true;
        if (ProductId.HasValue)
        {
            expression = expression.And(x => x.ProductId == ProductId);
        }
        if (LiveVideoId.HasValue)
        {
            expression = expression.And(x => x.LiveVideoId == LiveVideoId);
        }
        if (!string.IsNullOrEmpty(CreatorName))
        {
            expression = expression.And(x => EF.Functions.Like(x.CreatorName, $"%{CreatorName}%"));
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => EF.Functions.Like(x.Phone, $"%{Phone}%"));
        }

        return expression;
    }
}