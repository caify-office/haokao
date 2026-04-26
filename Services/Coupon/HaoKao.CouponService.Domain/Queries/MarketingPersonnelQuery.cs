using Girvs.Extensions;
using HaoKao.CouponService.Domain.Models;
using System;

namespace HaoKao.CouponService.Domain.Queries;

public class MarketingPersonnelQuery : QueryBase<MarketingPersonnel>
{
    /// <summary>
    /// 姓名
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [QueryCacheKey]
    public string TelPhone { get; set; }

    public override Expression<Func<MarketingPersonnel, bool>> GetQueryWhere()
    {
        Expression<Func<MarketingPersonnel, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (!string.IsNullOrEmpty(TelPhone))
        {
            expression = expression.And(x => x.TelPhone.Contains(TelPhone));
        }
        return expression;
    }
}