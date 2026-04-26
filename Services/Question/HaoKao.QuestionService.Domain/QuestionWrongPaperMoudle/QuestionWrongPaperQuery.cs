using System.ComponentModel.DataAnnotations;

namespace HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;

public class QuestionWrongPaperQuery : QueryBase<QuestionWrongPaper>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [Required, QueryCacheKey]
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [Required, QueryCacheKey]
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 组卷时间范围
    /// </summary>
    [QueryCacheKey]
    public int? CreateDateRange { get; set; }

    public override Expression<Func<QuestionWrongPaper, bool>> GetQueryWhere()
    {
        Expression<Func<QuestionWrongPaper, bool>> expression = x => x.SubjectId == SubjectId && x.CreatorId == CreatorId;

        if (CreateDateRange.HasValue)
        {
            var now = DateTime.Now;
            var startDate = now.AddDays(-CreateDateRange.Value);
            expression = expression.And(x => x.CreateTime >= startDate && x.CreateTime <= now);
        }

        return expression;
    }
}