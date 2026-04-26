using Girvs.Extensions;

namespace HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

public class ChapterNodeQuery : QueryBase<ChapterNode>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ParentId { get; set; }

    public override Expression<Func<ChapterNode, bool>> GetQueryWhere()
    {
        Expression<Func<ChapterNode, bool>> expression = x => true;

        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }
        if (ParentId.HasValue)
        {
            expression = expression.And(x => x.ParentId == ParentId);
        }
        else
        {
            expression = expression.And(x => !x.ParentId.HasValue);
        }

        return expression;
    }
}
