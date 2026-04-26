using HaoKao.LiveBroadcastService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveOnlineUserQuery : QueryBase<LiveOnlineUser>
{
    /// <summary>
    /// 直播Id
    /// </summary>
    [Required]
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

    public override Expression<Func<LiveOnlineUser, bool>> GetQueryWhere()
    {
        Expression<Func<LiveOnlineUser, bool>> expression = x => x.LiveId == LiveId;

        if (!string.IsNullOrEmpty(CreatorName))
        {
            expression = expression.And(x => x.CreatorName == CreatorName);
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone == Phone);
        }

        return expression;
    }
}