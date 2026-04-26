using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Repositories;

public interface ISubjectRepository : IRepository<Subject, Guid>
{
    void AddSort(SubjectSort sort);

    void UpdateSort(SubjectSort sort);
}