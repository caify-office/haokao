using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Repositories;

public class AnswerRecordRepository : Repository<AnswerRecord>, IAnswerRecordRepository
{
    public Task<int> GetSubjectQuestionCount(Guid subjectId, Guid userId)
    {
        var typeId = new Guid("68eae47e-3df7-4c78-9e73-0294bcbdd7ac");
        return Queryable.Where(x => x.SubjectId == subjectId && x.CreatorId == userId)
                        .Include(x => x.Questions)
                        .SelectMany(x => x.Questions
                                          .Where(y => y.QuestionTypeId != typeId && y.JudgeResult != ScoringRuleType.Missing)
                                          .Select(y => y.QuestionId))
                        .Distinct()
                        .CountAsync();
    }
}