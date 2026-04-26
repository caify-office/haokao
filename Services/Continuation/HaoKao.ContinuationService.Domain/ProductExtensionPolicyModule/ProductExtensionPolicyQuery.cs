namespace HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

public class ProductExtensionPolicyQuery : QueryBase<ProductExtensionPolicy>
{
    /// <summary>
    /// 策略名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [QueryCacheKey]
    public bool? IsEnable { get; set; }

    public override Expression<Func<ProductExtensionPolicy, bool>> GetQueryWhere()
    {
        Expression<Func<ProductExtensionPolicy, bool>> expression = x => !x.IsDelete;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => EF.Functions.Like(x.Name, $"%{Name}%"));
        }

        if (IsEnable.HasValue)
        {
            expression = expression.And(x => x.IsEnable == IsEnable);
        }

        return expression;
    }
}