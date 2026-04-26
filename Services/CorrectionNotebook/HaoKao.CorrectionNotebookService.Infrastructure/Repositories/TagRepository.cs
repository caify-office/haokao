using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Repositories;

public class TagRepository : Repository<Tag, Guid>, ITagRepository
{
    public async Task<IReadOnlyList<Tag>> GetByIdsAsync(IReadOnlyList<Guid> ids)
    {
        return await Queryable.Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public async Task<IReadOnlyList<Tag>> GetByUserAsync(Guid userId)
    {
        return await Queryable.Where(x => x.CreatorId == userId || x.IsBuiltIn).ToListAsync();
    }
}