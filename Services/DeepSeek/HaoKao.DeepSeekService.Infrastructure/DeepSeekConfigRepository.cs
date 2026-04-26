using AutoMapper;
using HaoKao.DeepSeekService.Domain;

namespace HaoKao.DeepSeekService.Infrastructure;

public class DeepSeekConfigRepository(DeepSeekDbContext dbContext, IMapper mapper) : IDeepSeekConfigRepository
{
    public Task<DeepSeekConfig> GetByTenantId(Guid tenantId)
    {
        return dbContext.DeepSeekConfigs.AsNoTracking().FirstOrDefaultAsync(x => x.TenantId == tenantId);
    }

    public Task SaveAsync(DeepSeekConfig entity)
    {
        var source = dbContext.DeepSeekConfigs.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id && x.TenantId == entity.TenantId);
        if (source == null)
        {
            dbContext.DeepSeekConfigs.Add(entity);
        }
        else
        {
            mapper.Map(entity, source);
            dbContext.DeepSeekConfigs.Update(entity);
        }
        return dbContext.SaveChangesAsync();
    }
}