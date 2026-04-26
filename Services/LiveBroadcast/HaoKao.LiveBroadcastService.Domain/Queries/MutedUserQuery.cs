using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class MutedUserQuery : QueryBase<MutedUser>
{
    /// <summary>
    /// 姓名
    /// </summary>
    [QueryCacheKey]
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    public override Expression<Func<MutedUser, bool>> GetQueryWhere()
    {
        Expression<Func<MutedUser, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(UserName))
        {
            expression = expression.And(x => x.UserName.Contains(UserName));
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone.Contains(Phone));
        }

        return expression;
    }
}