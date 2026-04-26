using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Queries;
using HaoKao.CampaignService.Domain.Repositories;

namespace HaoKao.CampaignService.Infrastructure.Repositories;

public class GiftBagRepository : Repository<GiftBag>, IGiftBagRepository
{
    public Task<GiftBag> GetByIdWithLogs(Guid id)
    {
        return Queryable.Include(x => x.GiftBagReceiveLogs).FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task DeleteByIds(IReadOnlyList<Guid> ids)
    {
        return Queryable.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
    }

    public Task UpdatePublished(IReadOnlyList<Guid> ids, bool isPublished)
    {
        return Queryable.Where(x => ids.Contains(x.Id))
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPublished, isPublished));
    }

    public async Task<IReadOnlyList<GiftBag>> GetPublishedGiftBagsByUser(GiftBagPublishedByUserQuery query)
    {
        return await Queryable.Include(query.Include)
                              .Where(query.Criteria)
                              .OrderBy(query.OrderBy)
                              .Select(query.Selector)
                              .ToListAsync();
    }
}