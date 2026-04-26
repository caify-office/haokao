namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

public class ProductExtensionRequestQuery : QueryBase<ProductExtensionRequest>
{
    /// <summary>
    /// 产品名称
    /// </summary>
    [QueryCacheKey]
    public string ProductName { get; set; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    [QueryCacheKey]
    public string StudentName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [QueryCacheKey]
    public ProductExtensionRequestState? AuditState { get; set; }

    /// <summary>
    /// 申请人Id
    /// </summary>
    [QueryCacheKey]
    public Guid? CreatorId { get; set; }

    public override Expression<Func<ProductExtensionRequest, bool>> GetQueryWhere()
    {
        Expression<Func<ProductExtensionRequest, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(ProductName))
        {
            expression = expression.And(x => EF.Functions.Like(x.ProductName, $"%{ProductName}%"));
        }
        if (!string.IsNullOrEmpty(StudentName))
        {
            expression = expression.And(x => EF.Functions.Like(x.StudentName, $"%{StudentName}%"));
        }
        if (AuditState.HasValue)
        {
            expression = expression.And(x => x.AuditState == AuditState);
        }
        if (CreatorId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == CreatorId);
        }

        return expression;
    }
}