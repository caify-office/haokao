using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Domain.Queries;

public class CourseAgreementQuery : QueryBase<CourseAgreement>
{
    /// <summary>
    /// 协议名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 协议类型
    /// </summary>
    [QueryCacheKey]
    public AgreementType? AgreementType { get; set; }

    /// <summary>
    /// Ids
    /// </summary>
    [QueryCacheKey]
    public IReadOnlyList<Guid> Ids { get; init; }

    public override Expression<Func<CourseAgreement, bool>> GetQueryWhere()
    {
        Expression<Func<CourseAgreement, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => EF.Functions.Like(x.Name, $"%{Name}%"));
        }
        if (AgreementType.HasValue)
        {
            expression = expression.And(x => x.AgreementType == AgreementType);
        }
        if (Ids?.Any() == true)
        {
            expression = expression.And(x => Ids.Contains(x.Id));
        }

        return expression;
    }
}