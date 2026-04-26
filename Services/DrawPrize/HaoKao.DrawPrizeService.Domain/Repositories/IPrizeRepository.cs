
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.Repositories;

public interface IPrizeRepository : IRepository<Prize>
{
    Task<int> GetPrizeCountAsync(Guid drawPrizeId);
}
