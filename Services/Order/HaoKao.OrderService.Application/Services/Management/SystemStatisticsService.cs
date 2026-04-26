using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 销售统计接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "销售统计",
    "61d4663b-502c-4bad-b38a-79bdaf97e26c",
    "32",
    SystemModule.SystemModule,
    3
)]
public class SystemStatisticsService(IOrderService service) : ISystemStatisticsService
{
    /// <summary>
    /// 获取销售统计列表
    /// </summary>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("销售统计", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public Task<SalesStatQueryViewModel> GetSalesStatList([FromBody] SalesStatQueryViewModel model)
    {
        return service.GetSalesStatList(model);
    }

    /// <summary>
    /// 获取销售统计详情列表
    /// </summary>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("销售统计详情", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public Task<SalesStatDetailQueryViewModel> GetSalesStatDetailList([FromBody] SalesStatDetailQueryViewModel model)
    {
        return service.GetSalesStatDetailList(model);
    }
}