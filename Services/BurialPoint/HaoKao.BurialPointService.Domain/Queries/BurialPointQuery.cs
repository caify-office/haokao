using Girvs.Extensions;
using HaoKao.BurialPointService.Domain.Entities;
using HaoKao.BurialPointService.Domain.Enums;

namespace HaoKao.BurialPointService.Domain.Queries;

public class BurialPointQuery : QueryBase<BurialPoint>
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 所属端
    /// </summary>
    [QueryCacheKey]
    public BelongingPortType? BelongingPortType { get; set; }
    public override Expression<Func<BurialPoint, bool>> GetQueryWhere()
    {
        Expression<Func<BurialPoint, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (BelongingPortType.HasValue)
        {
            expression = expression.And(x => x.BelongingPortType == BelongingPortType);
        }
        return expression;
    }
}
