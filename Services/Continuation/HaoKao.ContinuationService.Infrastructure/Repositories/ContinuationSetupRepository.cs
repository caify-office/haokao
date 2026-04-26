using HaoKao.ContinuationService.Domain.ContinuationSetupModule;

namespace HaoKao.ContinuationService.Infrastructure.Repositories;

public class ContinuationSetupRepository : Repository<ContinuationSetup>, IContinuationSetupRepository
{
    public IQueryable<ContinuationSetup> Query => Queryable.AsNoTracking();
}