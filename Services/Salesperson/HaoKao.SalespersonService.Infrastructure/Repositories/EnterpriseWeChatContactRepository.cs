using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Repositories;

namespace HaoKao.SalespersonService.Infrastructure.Repositories;

public class EnterpriseWeChatContactRepository(SalespersonDbContext dbContext) : Repository<EnterpriseWeChatContact>, IEnterpriseWeChatContactRepository
{
    public async Task<IReadOnlyList<EnterpriseWeChatContact>> GetByFollowUserId(string followUserId)
    {
        return await dbContext.EnterpriseWeChatContacts.AsNoTracking().Where(x => x.FollowUserId == followUserId).ToListAsync();
    }

    public override async Task<List<EnterpriseWeChatContact>> AddRangeAsync(List<EnterpriseWeChatContact> ts)
    {
        await dbContext.EnterpriseWeChatContacts.AddRangeAsync(ts);
        await dbContext.SaveChangesAsync();
        return ts;
    }

    public override Task DeleteRangeAsync(List<EnterpriseWeChatContact> ts)
    {
        return dbContext.EnterpriseWeChatContacts.Where(x => ts.Select(x => x.Id).Contains(x.Id)).ExecuteDeleteAsync();
    }
}