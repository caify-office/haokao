using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Queries;

public class RelatedProductQuery : QueryBase<RelatedProduct>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 关联对象产品id
    /// </summary>
    [QueryCacheKey]
    public Guid? RelatedTargetProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [QueryCacheKey]
    public string ProductName { get; set; }

    /// <summary>
    /// 关联对象产品名称
    /// </summary>
    [QueryCacheKey]
    public string RelatedTargetProducName { get; set; }

    public override Expression<Func<RelatedProduct, bool>> GetQueryWhere()
    {
        Expression<Func<RelatedProduct, bool>> expression = x => true;
        if (ProductId.HasValue)
        {
            expression = expression.And(x => x.ProductId == ProductId);
        }
        if (RelatedTargetProductId.HasValue)
        {
            expression = expression.And(x => x.RelatedTargetProductId == RelatedTargetProductId);
        }
        if (!string.IsNullOrEmpty(ProductName))
        {
            expression = expression.And(x => x.ProductName.Contains(ProductName));
        }

        if (!string.IsNullOrEmpty(RelatedTargetProducName))
        {
            expression = expression.And(x => x.RelatedTargetProducName.Contains(RelatedTargetProducName));
        }
        return expression;
    }
}