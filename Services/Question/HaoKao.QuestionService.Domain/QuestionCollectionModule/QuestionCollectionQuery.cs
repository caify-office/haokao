using Girvs.Infrastructure;

namespace HaoKao.QuestionService.Domain.QuestionCollectionModule;

public class QuestionCollectionQuery : QueryBase<QuestionCollection>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 试题类型
    /// </summary>
    [QueryCacheKey]
    public Guid? QuestionTypeId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [QueryCacheKey]
    public Guid CreatorId => EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();

    public override Expression<Func<QuestionCollection, bool>> GetQueryWhere()
    {
        Expression<Func<QuestionCollection, bool>> where = x => x.CreatorId == CreatorId;

        if (SubjectId.HasValue)
        {
            where = where.And(x => x.Question.SubjectId == SubjectId);
        }

        return where;
    }
}