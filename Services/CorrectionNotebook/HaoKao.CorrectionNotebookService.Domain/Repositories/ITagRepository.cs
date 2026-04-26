using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Repositories;

public interface ITagRepository : IRepository<Tag, Guid>
{
    Task<IReadOnlyList<Tag>> GetByIdsAsync(IReadOnlyList<Guid> ids);

    Task<IReadOnlyList<Tag>> GetByUserAsync(Guid userId);
}