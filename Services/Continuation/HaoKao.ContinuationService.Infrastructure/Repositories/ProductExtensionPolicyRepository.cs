using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

namespace HaoKao.ContinuationService.Infrastructure.Repositories;

public class ProductExtensionPolicyRepository : Repository<ProductExtensionPolicy>, IProductExtensionPolicyRepository
{
    public IQueryable<ProductExtensionPolicy> Query => Queryable;
}