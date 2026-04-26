using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveCoupon;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveCouponWebService
    (
    ILiveCouponService service,
    ILiveAdministratorWebService liveAdministratorWebService
) : ILiveCouponWebService
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    /// <returns></returns>
    [HttpGet]
    public Task<LiveCouponQueryViewModel> Get([FromQuery] LiveCouponQueryViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }


    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    public async Task ShelvesUp(Guid[] id)
    {
        var isAdmin= await liveAdministratorWebService.IsLiveAdmin();
        if (!isAdmin) 
        {
            throw new GirvsException("当前用户没有上架操作权限");
        }
        await service.ShelvesUp(id);
    }

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    public async Task ShelvesDown([FromBody] Guid[] id)
    {
        var isAdmin = await liveAdministratorWebService.IsLiveAdmin();
        if (!isAdmin)
        {
            throw new GirvsException("当前用户没有下架操作权限");
        }
        await service.ShelvesDown(id);
    }
}