using Girvs.Extensions;
using HaoKao.PaperTempleteService.Domain.Entities;

namespace HaoKao.PaperTempleteService.Domain.Queries;

public class PaperTempleteQuery : QueryBase<PaperTemplete>
{
    [QueryCacheKey]
    public string TempleteName { get; set; }

    [QueryCacheKey]
    public string SubjectId { get; set; }

    public override Expression<Func<PaperTemplete, bool>> GetQueryWhere()
    {
        Expression<Func<PaperTemplete, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(TempleteName))
        {
            expression = expression.And(x => x.TempleteName.Contains(TempleteName));
        }

        if (!string.IsNullOrEmpty(SubjectId))
        {
            expression = expression.And(x => x.SuitableSubjects.Contains(SubjectId));
        }

        return expression;
    }
}