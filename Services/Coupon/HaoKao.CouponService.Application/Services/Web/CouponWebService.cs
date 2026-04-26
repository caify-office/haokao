using Girvs.AuthorizePermission.Enumerations;
using HaoKao.Common;
using HaoKao.CouponService.Application.Services.Management;
using HaoKao.CouponService.Application.ViewModels.Coupon;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.CouponService.Application.Services.Web;

/// <summary>
/// 优惠券接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CouponWebService(ICouponService service) : ICouponWebService
{
    private readonly ICouponService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据主键数组获取指定
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<List<BrowseCouponViewModel>> GetByIds(Guid[] ids)
    {
        return _service.GetByIds(ids);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<CouponQueryViewModel> Get([FromQuery] CouponQueryViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }
}