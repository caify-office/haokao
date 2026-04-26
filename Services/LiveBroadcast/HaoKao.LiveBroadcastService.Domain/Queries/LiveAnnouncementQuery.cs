using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveAnnouncementQuery : QueryBase<LiveAnnouncement>
{
    /// <summary>
    /// 内容
    /// </summary>
    [QueryCacheKey]
    public string Content { get; set; }

    public override Expression<Func<LiveAnnouncement, bool>> GetQueryWhere()
    {
        Expression<Func<LiveAnnouncement, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Content))
        {
            expression = expression.And(x => x.Content.Contains(Content));
        }
        return expression;
    }
}