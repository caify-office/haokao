using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Web;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveProductPackageWeChatService(ILiveProductPackageWebService service) : ILiveProductPackageWeChatService
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    /// <returns></returns>
    [HttpGet]
    public Task<LiveProductPackageQueryViewModel> Get([FromQuery] LiveProductPackageQueryViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }


    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    public  Task ShelvesUp(Guid[] id)
    {
        return service.ShelvesUp(id);
    }

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    public  Task ShelvesDown([FromBody] Guid[] id)
    {
        return service.ShelvesDown(id);
    }
}