using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.Services.Web;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;
using HaoKao.LiveBroadcastService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 直播预约接口服务-微信小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveReservationWeChatService(ILiveReservationService service, ILiveReservationWebService webService) : ILiveReservationWeChatService
{
    private readonly ILiveReservationWebService _webService = webService ?? throw new ArgumentNullException(nameof(webService));

    /// <summary>
    /// 创建直播预约
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> Create([FromBody] CreateLiveReservationViewModel model)
    {
        model.ReservationSource = ReservationSource.WeChat;
        return await service.Create(model);
    }

    /// <summary>
    /// 我的直播预约
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<BrowseLiveReservationViewModel>> MyLiveReservation()
    {
        return _webService.MyLiveReservation();
    }

    /// <summary>
    /// 预约人数统计
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public Task<dynamic> LiveReservationCount(Guid[] productIds)
    {
        return _webService.LiveReservationCount(productIds);
    }
}