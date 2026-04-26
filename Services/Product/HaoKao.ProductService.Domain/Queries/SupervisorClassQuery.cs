using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Queries;

public class SupervisorClassQuery : QueryBase<SupervisorClass>
{
    /// <summary>
    /// 班级名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [QueryCacheKey]
    public int? Year { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    [QueryCacheKey]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [QueryCacheKey]
    public string ProductName { get; set; }

    public override Expression<Func<SupervisorClass, bool>> GetQueryWhere()
    {
        Expression<Func<SupervisorClass, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }

        if (Year.HasValue)
        {
            expression = expression.And(x => x.Year == Year);
        }

        if (ProductId.HasValue)
        {
            expression = expression.And(x => x.ProductId == ProductId);
        }


        if (!string.IsNullOrEmpty(ProductName))
        {
            expression = expression.And(x => x.ProductName.Contains(ProductName));
        }

        return expression;
    }
}