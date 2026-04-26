using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Queries;

public record ExamLevelQuery(Guid? UserId)
{
    public Expression<Func<ExamLevel, bool>> Criteria
    {
        get
        {
            if (UserId == null)
            {
                return x => x.IsBuiltIn;
            }
            return x => x.IsBuiltIn || x.CreatorId == UserId;
        }
    }

#pragma warning disable CA1822 // 将成员标记为 static
    public Expression<Func<ExamLevel, object>> OrderBy => x => x.CreateTime;
#pragma warning restore CA1822 // 将成员标记为 static
}