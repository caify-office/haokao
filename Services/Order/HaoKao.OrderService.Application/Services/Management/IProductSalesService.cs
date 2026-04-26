using HaoKao.OrderService.Domain.Queries;
using HaoKao.OrderService.Domain.ValueObjects;

namespace HaoKao.OrderService.Application.Services.Management;

public interface IProductSalesService : IAppWebApiService, IManager
{
    Task<ProductSalesStatList> GetProductSalesStatListAsync(ProducSalesQuery query);
}