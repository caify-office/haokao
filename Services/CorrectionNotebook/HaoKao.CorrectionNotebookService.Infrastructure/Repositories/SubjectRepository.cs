using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Repositories;

public class SubjectRepository(CorrectionNotebookDbContext dbContext) : Repository<Subject, Guid>, ISubjectRepository
{
    public void AddSort(SubjectSort sort)
    {
        dbContext.SubjectSorts.Add(sort);
    }

    public void UpdateSort(SubjectSort sort)
    {
        dbContext.SubjectSorts.Update(sort);
    }
}