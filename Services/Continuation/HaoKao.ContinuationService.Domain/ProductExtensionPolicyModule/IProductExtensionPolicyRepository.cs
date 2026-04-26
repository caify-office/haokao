namespace HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

public interface IProductExtensionPolicyRepository : IRepository<ProductExtensionPolicy>
{
    IQueryable<ProductExtensionPolicy> Query { get; }
}