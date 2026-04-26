using Girvs.Extensions;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.Queries;

public class PrizeQuery : QueryBase<Prize>
{
    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    [QueryCacheKey]
    public Guid? DrawPrizeId { get; set; }
    public override Expression<Func<Prize, bool>> GetQueryWhere()
    {
    Expression<Func<Prize, bool>> expression = x => true;
    if (DrawPrizeId.HasValue)
    {
        expression = expression.And(x => x.DrawPrizeId == DrawPrizeId);
    }
    return expression;
    }
}
