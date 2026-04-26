namespace HaoKao.ContinuationService.Domain.ContinuationAuditModule;

public class ContinuationAuditQuery : QueryBase<ContinuationAudit>
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
    public AuditState? AuditState { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [QueryCacheKey]
    public Guid? CreatorId { get; set; }

    public override Expression<Func<ContinuationAudit, bool>> GetQueryWhere()
    {
        Expression<Func<ContinuationAudit, bool>> expression = x => true;

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