using HaoKao.DrawPrizeService.Domain.Entities;
using System.Threading.Tasks;

namespace HaoKao.DrawPrizeService.Infrastructure.Repositories;

public class PrizeRepository : Repository<Prize>, IPrizeRepository
{
    public Task<int> GetPrizeCountAsync(Guid drawPrizeId)
    {
        return Queryable.CountAsync(x => x.DrawPrizeId == drawPrizeId);
    }
}