using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Enums;

namespace HaoKao.CorrectionNotebookService.Domain.Queries;

public record QuestionQuery(
    Guid SubjectId,
    Guid UserId,
    MasteryDegree? MasteryDegree,
    int? CreateTime,
    IReadOnlyList<Guid> TagIds,
    int PageSize,
    int PageIndex
)
{
    private readonly int _pageSize = PageSize < 0 ? 10 : PageSize;

    private readonly int _pageIndex = PageIndex < 1 ? 1 : PageIndex;

    public int Take => _pageSize;

    public int Skip => (_pageIndex - 1) * _pageSize;

    [JsonIgnore]
    public Expression<Func<Question, bool>> Criteria
    {
        get
        {
            Expression<Func<Question, bool>> expr = x => x.CreatorId == UserId && x.SubjectId == SubjectId;

            if (TagIds != null && TagIds.Any())
            {
                expr = expr.And(x => x.Tags.Any(y => TagIds.Contains(y.TagId)));
            }
            if (CreateTime.HasValue)
            {
                var time = DateTime.Now.AddDays(-CreateTime.Value);
                expr = expr.And(x => x.CreateTime >= time);
            }
            if (MasteryDegree.HasValue)
            {
                expr = expr.And(x => x.MasteryDegree == MasteryDegree.Value);
            }

            return expr;
        }
    }

#pragma warning disable CA1822 // 将成员标记为 static

    [JsonIgnore]
    public Expression<Func<Question, object>> OrderBy => x => x.CreateTime;

#pragma warning restore CA1822 // 将成员标记为 static
}