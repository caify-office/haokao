using HaoKao.Common.Enums;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Domain.Queries;

public class ProductQuery : QueryBase<Product>
{
    /// <summary>
    /// 产品名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    [QueryCacheKey]
    public ProductType? ProductType { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [QueryCacheKey]
    public string Year { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [QueryCacheKey]
    public bool? Enable { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    [QueryCacheKey]
    public bool? IsShelves { get; set; }

    /// <summary>
    /// 过滤Id数组
    /// </summary>
    [QueryCacheKey]
    public string ExcludeIds { get; set; }

    /// <summary>
    /// 是否需要签署协议
    /// </summary>
    [QueryCacheKey]
    public bool? IsNeedAgreement { get; set; }

    /// <summary>
    /// 是否过期 (true:返回已过期优惠卷，false：返回未过期优惠卷，不传：过期和未过期的都返回)
    /// </summary>
    [QueryCacheKey]
    public bool? IsExpired { get; set; }

    /// <summary>
    /// 是否体验产品包
    /// </summary>
    [QueryCacheKey]
    public bool? IsExperience { get; set; }

    public override Expression<Func<Product, bool>> GetQueryWhere()
    {
        Expression<Func<Product, bool>> expression = x => true;

        if (ProductType.HasValue)
        {
            expression = expression.And(x => x.ProductType == ProductType);
        }
        if (!string.IsNullOrEmpty(Year))
        {
            expression = expression.And(x => x.Year == Year);
        }
        if (Enable.HasValue)
        {
            expression = expression.And(x => x.Enable == Enable);
        }
        if (IsShelves.HasValue)
        {
            expression = expression.And(x => x.IsShelves == IsShelves);
        }

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }

        if (!string.IsNullOrEmpty(ExcludeIds))
        {
            expression = expression.And(x => !ExcludeIds.Contains(x.Id.ToString()));
        }
        if (IsNeedAgreement.HasValue)
        {
            if (IsNeedAgreement.Value)
            {
                expression = expression.And(x => x.Agreement.HasValue);
            }
            else
            {
                expression = expression.And(x => !x.Agreement.HasValue);
            }
        }

        if (IsExpired.HasValue)
        {
            var now = DateTime.Now;
            if (IsExpired.Value)
            {
                expression = expression.And(x => (x.ExpiryTimeTypeEnum == ExpiryTimeTypeEnum.Date && x.ExpiryTime < now)
                                              || x.ExpiryTimeTypeEnum == ExpiryTimeTypeEnum.Day);
            }
            else
            {
                expression = expression.And(x => (x.ExpiryTimeTypeEnum == ExpiryTimeTypeEnum.Date && x.ExpiryTime >= now)
                                              || x.ExpiryTimeTypeEnum == ExpiryTimeTypeEnum.Day);
            }
        }

        if (IsExperience.HasValue)
        {
            expression = expression.And(x => x.IsExperience == IsExperience);
        }

        return expression;
    }
}