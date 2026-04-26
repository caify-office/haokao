using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HaoKao.ProductService.Domain.Queries;

public class ProductPackageQuery : QueryBase<ProductPackage>
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    [QueryCacheKey]
    public ProductType? ProductType { get; set; }

    /// <summary>
    /// 产品Id(返回包含这个产品id的产品包)
    /// </summary>
    [QueryCacheKey]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 所属年份
    /// </summary>
    [QueryCacheKey]
    public DateTime? Year { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [QueryCacheKey]
    public bool? Enable { get; set; }

    /// <summary>
    /// 上架
    /// </summary>
    [QueryCacheKey]
    public bool? Shelves { get; set; }

    /// <summary>
    /// 是否排除过期的产品包
    /// </summary>
    [QueryCacheKey]
    public bool? IsRemoveExpiry { get; set; }

    /// <summary>
    /// 未登录时，不会使用租户id进行筛选，需要自己手动添加筛选
    /// </summary>
    [QueryCacheKey]
    public Guid? TenantId => EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();

    /// <summary>
    /// 过滤Id数组
    /// </summary>
    [QueryCacheKey]
    public string ExcludeIds { get; set; }

    /// <summary>
    /// 对比
    /// </summary>
    [QueryCacheKey]
    public bool? Contrast { get; set; }

    /// <summary>
    /// 是否体验产品包
    /// </summary>
    [QueryCacheKey]
    public bool? IsExperience { get; set; }

    public override Expression<Func<ProductPackage, bool>> GetQueryWhere()
    {
        Expression<Func<ProductPackage, bool>> expression = x => true;
        if (!Name.IsNullOrEmpty())
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (ProductType.HasValue)
        {
            if (ProductType == Enums.ProductType.Course)
            {
                expression = expression.And(x => x.ProductType == Enums.ProductType.Course || x.ProductType == Enums.ProductType.IntelligentAssistanceCourse);
            }
            else
            {
                expression = expression.And(x => x.ProductType == ProductType);
            }
        }

        if (ProductId.HasValue)
        {
            expression = expression.And(x => EF.Functions.JsonContains(x.ProductList, $"\"{ProductId}\""));
        }
        if (Year.HasValue)
        {
            expression = expression.And(x => x.Year.Year == Year.Value.Year);
        }
        if (Enable.HasValue)
        {
            expression = expression.And(x => x.Enable == Enable);
        }
        if (Shelves.HasValue)
        {
            expression = expression.And(x => x.Shelves == Shelves);
        }
        if (TenantId.HasValue)
        {
            expression = expression.And(x => x.TenantId == TenantId);
        }
        if (IsRemoveExpiry.HasValue && IsRemoveExpiry.Value)
        {
            var now = DateTime.Now;
            expression = expression.And(x => x.ExpiryTime >= now);
        }

        if (!string.IsNullOrEmpty(ExcludeIds))
        {
            expression = expression.And(x => !ExcludeIds.Contains(x.Id.ToString()));
        }
        if (Contrast.HasValue)
        {
            expression = expression.And(x => x.Contrast == Contrast);
        }
        if (IsExperience.HasValue)
        {
            expression = expression.And(x => x.IsExperience == IsExperience);
        }
        return expression;
    }
}