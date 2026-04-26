using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

namespace HaoKao.ContinuationService.Infrastructure.Repositories;

public class ProductExtensionRequestRepository : Repository<ProductExtensionRequest>, IProductExtensionRequestRepository
{
    public IQueryable<ProductExtensionRequest> Query => Queryable;
}