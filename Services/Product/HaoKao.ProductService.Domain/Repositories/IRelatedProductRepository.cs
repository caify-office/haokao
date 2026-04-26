using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Repositories;

public interface IRelatedProductRepository : IRepository<RelatedProduct>
{
    Task<List<Product>> GetRelatedProductByIds(Guid[] ids);
}
