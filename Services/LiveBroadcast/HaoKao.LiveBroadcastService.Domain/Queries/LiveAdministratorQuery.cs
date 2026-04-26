using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class LiveAdministratorQuery : QueryBase<LiveAdministrator>
{
    /// <summary>
    /// 姓名
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    public override Expression<Func<LiveAdministrator, bool>> GetQueryWhere()
    {
        Expression<Func<LiveAdministrator, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone.Contains(Phone));
        }
        return expression;
    }
}