using Girvs.Extensions;
using HaoKao.KnowledgePointService.Domain.Entities;
using System;

namespace HaoKao.KnowledgePointService.Domain.Queries;

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
    public override Expression<Func<KnowledgePoint, bool>> GetQueryWhere()
    {
        Expression<Func<KnowledgePoint, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (ChapterNodeId.HasValue)
        {
            expression = expression.And(x => x.ChapterNodeId==ChapterNodeId);
        }
        return expression;
    }
}