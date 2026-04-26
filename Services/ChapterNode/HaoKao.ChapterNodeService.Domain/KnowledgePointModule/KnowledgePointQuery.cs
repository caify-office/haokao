using Girvs.Extensions;

namespace HaoKao.ChapterNodeService.Domain.KnowledgePointModule;

public class KnowledgePointQuery : QueryBase<KnowledgePoint>
{
    /// <summary>
    /// 知识点名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 章节id
    /// </summary>
    [QueryCacheKey]
    public Guid? ChapterNodeId { get; set; }

    public Guid[] ChildrenChapterNodeIds { get; set; }

    /// <summary>
    /// 章节id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    public Guid[] SubjectIdAllChapterNodeIds { get; set; }

    public override Expression<Func<KnowledgePoint, bool>> GetQueryWhere()
    {
        Expression<Func<KnowledgePoint, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (ChapterNodeId.HasValue)
        {
            Expression<Func<KnowledgePoint, bool>> expression1 = x => x.ChapterNodeId == ChapterNodeId;
            expression1 = expression1.Or(x => ChildrenChapterNodeIds.Contains(x.ChapterNodeId.Value));
            expression = expression.And(expression1);
        }
        if (SubjectId.HasValue)
        {
            Expression<Func<KnowledgePoint, bool>> expression1 = x => false;
            expression1 = expression1.Or(x => SubjectIdAllChapterNodeIds.Contains(x.ChapterNodeId.Value));
            expression = expression.And(expression1);
        }
        return expression;
    }
}