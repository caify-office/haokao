using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.Repositories;

public interface IDrawPrizeRepository : IRepository<DrawPrize>
{
    Task UpdateEnableByIds(ICollection<Guid> ids, bool state);

    Task DeleteByIds(IReadOnlyList<Guid> ids);
}
