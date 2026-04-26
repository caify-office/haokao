using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Domain.Queries;

public class StudentAgreementQuery : QueryBase<StudentAgreement>
{
    /// <summary>
    /// 协议Id
    /// </summary>
    [QueryCacheKey]
    public Guid? AgreementId { get; set; }

    /// <summary>
    /// 协议名称
    /// </summary>
    [QueryCacheKey]
    public string AgreementName { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 学员名称
    /// </summary>
    [QueryCacheKey]
    public string StudentName { get; set; }

    /// <summary>
    /// 签署人Id
    /// </summary>
    [QueryCacheKey]
    public Guid? CreatorId { get; set; }

    public override Expression<Func<StudentAgreement, bool>> GetQueryWhere()
    {
        Expression<Func<StudentAgreement, bool>> expression = x => true;

        if (AgreementId.HasValue)
        {
            expression = expression.And(x => x.AgreementId == AgreementId.Value);
        }
        if (!string.IsNullOrEmpty(AgreementName))
        {
            expression = expression.And(x => EF.Functions.Like(x.AgreementName, $"%{AgreementName}%"));
        }
        if (!string.IsNullOrEmpty(StudentName))
        {
            expression = expression.And(x => EF.Functions.Like(x.StudentName, $"%{StudentName}%"));
        }
        if (ProductId.HasValue)
        {
            expression = expression.And(x => x.ProductId == ProductId.Value);
        }
        if (CreatorId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == CreatorId.Value);
        }

        return expression;
    }
}