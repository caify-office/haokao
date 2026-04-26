using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Queries;

namespace HaoKao.CorrectionNotebookService.Domain.Repositories;

public interface IExamLevelRepository : IRepository<ExamLevel, Guid>
{
    Task<ExamLevel> GetWithSubjectByUserAsync(Guid id, Guid userId);

    Task<IReadOnlyList<ExamLevel>> GetListByUserAsync(ExamLevelQuery query);
}