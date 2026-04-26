namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

public interface IProductExtensionRequestRepository : IRepository<ProductExtensionRequest>
{
    IQueryable<ProductExtensionRequest> Query { get; }
}