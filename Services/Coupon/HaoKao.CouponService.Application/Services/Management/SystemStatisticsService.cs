using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.CouponService.Application.Services.Management;

/// <summary>
/// 实名销售统计
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "实名销售统计",
    "5088e45a-6d6e-4a3d-b893-f8b517b22124",
    "32",
    SystemModule.SystemModule,
    3
)]
public class SystemStatisticsService(IUserCouponPerformanceService service) : ISystemStatisticsService
{
    /// <summary>
    /// 实名销售统计
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("实名销售统计", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public Task<QuerySalesPerformanceStatViewModel> GetStaticByPersonUerId([FromBody] QuerySalesPerformanceStatViewModel model)
    {
        return service.GetStaticByPersonUerId(model);
    }
}