using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveCommentQuery : QueryBase<LiveComment>
{
    /// <summary>
    /// 直播Id
    /// </summary>
    [QueryCacheKey]
    public Guid LiveId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [QueryCacheKey]
    public string CreatorName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <summary>
    /// 评分
    /// </summary>
    [QueryCacheKey]
    public int? Rating { get; set; }

    public override Expression<Func<LiveComment, bool>> GetQueryWhere()
    {
        Expression<Func<LiveComment, bool>> expression = x => x.LiveId == LiveId;

        if (!string.IsNullOrEmpty(CreatorName))
        {
            expression = expression.And(x => x.CreatorName == CreatorName);
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone == Phone);
        }
        if (Rating.HasValue)
        {
            expression = expression.And(x => x.Rating == Rating.Value);
        }

        return expression;
    }
}