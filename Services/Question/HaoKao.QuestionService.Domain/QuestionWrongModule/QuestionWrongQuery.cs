using Girvs.Infrastructure;

namespace HaoKao.QuestionService.Domain.QuestionWrongModule;

public class QuestionWrongQuery : QueryBase<QuestionWrong>
{
    /// <summary>
    /// 试题类型
    /// </summary>
    [QueryCacheKey]
    public Guid? QuestionTypeId { get; set; }

    /// <summary>
    /// 章节Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ChapterNodeId { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [QueryCacheKey]
    public static Guid CreatorId => EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();

    /// <summary>
    /// 试题类型Id集合
    /// </summary>
    public IReadOnlyList<Guid> QuestionTypeIds { get; set; }

    /// <summary>
    /// 试题类型Id集合缓存Key
    /// </summary>
    [QueryCacheKey]
    public string QuestionTypeIdsCacheKey => string.Join(",", QuestionTypeIds);

    /// <summary>
    /// 是否激活状态的错题, true 待消灭, false 被消灭
    /// </summary>
    [QueryCacheKey]
    public bool? IsActive { get; set; }

    public override Expression<Func<QuestionWrong, bool>> GetQueryWhere()
    {
        Expression<Func<QuestionWrong, bool>> where = x => x.CreatorId == CreatorId;

        if (SubjectId.HasValue)
        {
            where = where.And(x => x.Question.SubjectId == SubjectId);
        }

        if (ChapterNodeId.HasValue)
        {
            where = where.And(x => x.Question.ChapterId == ChapterNodeId);
        }

        if (QuestionTypeId.HasValue)
        {
            where = where.And(x => x.QuestionTypeId == QuestionTypeId || x.ParentQuestionTypeId == QuestionTypeId);
        }

        if (QuestionTypeIds != null && QuestionTypeIds.Any())
        {
            where = where.And(x => QuestionTypeIds.Contains(x.QuestionTypeId) || QuestionTypeIds.Contains(x.ParentQuestionTypeId));
        }

        if (IsActive.HasValue)
        {
            where = where.And(x => x.IsActive == IsActive);
        }

        return where;
    }
}