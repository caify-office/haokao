using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Queries;

public class UserAnswerRecordQuery : QueryBase<UserAnswerRecord>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    [QueryCacheKey]
    public Guid? QuestionCategoryId { get; set; }

    /// <summary>
    /// 答题类型
    /// </summary>
    [QueryCacheKey]
    public SubmitAnswerType? AnswerType { get; set; }

    /// <summary>
    /// 答题标识符 章节Id，或试卷Id，每日一练和消灭错题 为Guid.Empty
    /// </summary>
    public IReadOnlyList<Guid> RecordIdentifier { get; set; } = [];

    /// <summary>
    /// 解决List对象的缓存问题
    /// </summary>
    [QueryCacheKey]
    public string RecordIdentifierMd5 => string.Join(',', RecordIdentifier).ToMd5();

    /// <summary>
    /// 答题开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// 答题结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndDateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [QueryCacheKey]
    public Guid? UserId { get; set; }

    public override Expression<Func<UserAnswerRecord, bool>> GetQueryWhere()
    {
        Expression<Func<UserAnswerRecord, bool>> expression = _ => true;

        if (UserId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == UserId.Value);
        }

        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }

        if (QuestionCategoryId.HasValue)
        {
            expression = expression.And(x => x.QuestionCategoryId == QuestionCategoryId);
        }

        if (AnswerType.HasValue)
        {
            expression = expression.And(x => x.AnswerType == AnswerType);
        }

        if (RecordIdentifier.Any())
        {
            expression = expression.And(x => RecordIdentifier.Contains(x.RecordIdentifier));
        }

        if (StartDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartDateTime);
        }

        if (EndDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime <= EndDateTime);
        }

        return expression;
    }
}