using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Web;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 视频直播接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
public class LiveVideoCallBackService(
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications) : ILiveVideoCallBackService
{
    #region 初始参数

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    #endregion

    #region 回调接口

    [HttpGet]
    [AllowAnonymous]
    public async Task<string> LiveStreamsNotify([FromQuery] TlCallBackViewModel model)
    {
        // URL解码查询字符串
        var decodedQuery = HttpUtility.UrlDecode(model.Usrargs);

        // 解析查询字符串到NameValueCollection
        var queryParams = HttpUtility.ParseQueryString(decodedQuery);

        // 从NameValueCollection中获取用户参数
        var tenantId = queryParams.Get("tenantId");
        var str = queryParams.Get("liveVideoId");
        var liveVideoId = queryParams.Get("liveVideoId").ToGuid();

        //重置请求头
        var headDictionary = new Dictionary<string, string>();
        headDictionary.Add(GirvsIdentityClaimTypes.TenantId, tenantId);
        EngineContext.Current.ClaimManager.SetFromDictionary(headDictionary);
        var liveStatus = model.Action == "publish" ? LiveStatus.LiveStreaming : LiveStatus.Ended;
        var command = new SetLiveVideoStatusCommand(liveVideoId, liveStatus);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        //var command=new SetLiveVideoStatusCommand(m)
        var timestamp = EngineContext.Current.HttpContext.Request.Headers["ALI-LIVE-TIMESTAMP"];
        var signatrue = EngineContext.Current.HttpContext.Request.Headers["ALI-LIVE-SIGNATURE"];
        return "SUCCESS";
    }

    #endregion
}