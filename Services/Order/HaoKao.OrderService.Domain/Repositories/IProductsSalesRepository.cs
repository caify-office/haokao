using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Queries;
using HaoKao.OrderService.Domain.ValueObjects;

namespace HaoKao.OrderService.Domain.Repositories;

public interface IProductSalesRepository : IRepository<ProductSales>
{
    Task<ProductSalesStatList> GetProductSalesStatListAsync(ProducSalesQuery query);
}