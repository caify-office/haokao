namespace HaoKao.ContinuationService.Domain.ContinuationSetupModule;

public interface IContinuationSetupRepository : IRepository<ContinuationSetup>
{
    IQueryable<ContinuationSetup> Query { get; }
}