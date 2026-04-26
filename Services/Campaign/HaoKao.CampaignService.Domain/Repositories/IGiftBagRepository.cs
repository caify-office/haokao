using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Queries;

namespace HaoKao.CampaignService.Domain.Repositories;

public interface IGiftBagRepository : IRepository<GiftBag>
{
    Task<GiftBag> GetByIdWithLogs(Guid id);

    Task DeleteByIds(IReadOnlyList<Guid> ids);

    Task UpdatePublished(IReadOnlyList<Guid> ids, bool isPublished);

    Task<IReadOnlyList<GiftBag>> GetPublishedGiftBagsByUser(GiftBagPublishedByUserQuery query);
}