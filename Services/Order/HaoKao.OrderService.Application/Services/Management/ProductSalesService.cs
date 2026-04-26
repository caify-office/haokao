using HaoKao.OrderService.Domain.Queries;
using HaoKao.OrderService.Domain.Repositories;
using HaoKao.OrderService.Domain.ValueObjects;

namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 产品销售统计服务
/// </summary>
/// <param name="repository"></param>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "产品销售统计",
    "067568a1-253d-7f2a-8000-f733d0f0af31",
    "1024",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    1
)]
public class ProductSalesService(IProductSalesRepository repository) : IProductSalesService
{
    /// <summary>
    /// 获取产品销售统计列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<ProductSalesStatList> GetProductSalesStatListAsync([FromQuery] ProducSalesQuery query)
    {
        if (query.IsExport == true)
        {
            query.StartTime = DateTime.Parse("2000-01-01 00:00:00");
            query.EndTime = DateTime.Now.AddDays(1);
        }
        return repository.GetProductSalesStatListAsync(query);
    }
}