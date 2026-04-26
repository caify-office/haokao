using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Repositories;

public interface IAnswerRecordRepository : IRepository<AnswerRecord>
{
    Task<int> GetSubjectQuestionCount(Guid subjectId, Guid userId);
}