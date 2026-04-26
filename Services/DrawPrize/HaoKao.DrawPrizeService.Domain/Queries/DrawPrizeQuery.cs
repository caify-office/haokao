using Girvs.Extensions;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.Queries;

public class DrawPrizeQuery : QueryBase<DrawPrize>
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string Title { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [QueryCacheKey]
    public DrawPrizeType? DrawPrizeType { get; set; }
    public override Expression<Func<DrawPrize, bool>> GetQueryWhere()
    {
        Expression<Func<DrawPrize, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Title))
        {
            expression = expression.And(x => x.Title.Contains(Title));
        }
        if (DrawPrizeType.HasValue)
        {
            expression = expression.And(x => x.DrawPrizeType == DrawPrizeType);
        }
        return expression;
    }
}
