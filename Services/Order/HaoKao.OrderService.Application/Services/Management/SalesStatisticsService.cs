namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 订单表接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "销售统计",
    "8723685d-d6bb-0b60-cf66-6eea14e5ed86",
    "1024",
    SystemModule.ExtendModule2,
    1
)]
public class SalesStatisticsService : ISalesStatisticsService
{
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public bool Get()
    {
        return true;
    }
}