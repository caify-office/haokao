using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

/// <summary>
/// 直播消息接口-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveMessageWebService(ILiveMessageService service) : ILiveMessageWebService
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<QueryLiveMessageViewModel> Get([FromQuery] QueryLiveMessageViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }

    /// <summary>
    /// 获取直播间置顶的消息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<PinTopMessageOutput> GetPinedMessage([FromQuery] PinTopMessageRequest request)
    {
        return service.GetPinedMessage(request);
    }

    /// <summary>
    /// 删除直播消息
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id:guid}")]
    public Task Delete(Guid id)
    {
        return service.Delete(id);
    }

    /// <summary>
    /// 产品上架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpGet("{liveId:guid}")]
    public Task ProductOnSell(Guid liveId, Guid[] ids)
    {
        return service.ProductOnSell(liveId,ids);
    }

    /// <summary>
    /// 优惠券领取通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    [HttpGet("{liveId:guid}")]
    public Task CouponPickUp(Guid liveId, Guid[] ids)
    {
        return service.CouponPickUp(liveId,ids);
    }
}