using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Queries;

public class ProductKnowledgeAnswerRecordQuery : QueryBase<ProductKnowledgeAnswerRecord>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    [QueryCacheKey]
    public Guid ProductId { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ChapterId { get; set; }

    /// <summary>
    /// 小节Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SectionId { get; set; }

    /// <summary>
    /// 掌握程度
    /// </summary>
    [QueryCacheKey]
    public MasteryLevel? MasteryLevel { get; set; }

    /// <summary>
    /// 知识点Ids
    /// </summary>
    public IReadOnlyList<Guid> KnowledgePointIds { get; set; }

    /// <summary>
    /// 知识点Ids缓存Key
    /// </summary>
    [QueryCacheKey]
    public string KnowledgePointIdsCacheKey => string.Join(",", KnowledgePointIds).ToMd5();

    /// <summary>
    /// 用户Id
    /// </summary>
    [QueryCacheKey]
    public Guid CreatorId { get; set; }

    public override Expression<Func<ProductKnowledgeAnswerRecord, bool>> GetQueryWhere()
    {
        Expression<Func<ProductKnowledgeAnswerRecord, bool>> expression = x => x.ProductId == ProductId && x.CreatorId == CreatorId;

        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }

        if (ChapterId.HasValue)
        {
            expression = expression.And(x => x.ChapterId == ChapterId);
        }

        if (SectionId.HasValue)
        {
            expression = expression.And(x => x.SectionId == SectionId);
        }

        if (MasteryLevel.HasValue)
        {
            expression = expression.And(x => x.MasteryLevel == MasteryLevel);
        }

        if (KnowledgePointIds != null && KnowledgePointIds.Any())
        {
            expression = expression.And(x => KnowledgePointIds.Contains(x.KnowledgePointId));
        }

        return expression;
    }
}