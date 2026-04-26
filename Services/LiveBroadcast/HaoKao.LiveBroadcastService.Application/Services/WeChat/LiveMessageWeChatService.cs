using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 直播消息接口-微信小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveMessageWeChatService(ILiveMessageService service) : ILiveMessageWeChatService
{
    #region 初始参数

    private readonly ILiveMessageService _service = service ?? throw new ArgumentNullException(nameof(service));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<QueryLiveMessageViewModel> Get([FromQuery] QueryLiveMessageViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }

    /// <summary>
    /// 获取直播间置顶的消息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<PinTopMessageOutput> GetPinedMessage([FromQuery] PinTopMessageRequest request)
    {
        return _service.GetPinedMessage(request);
    }

    #endregion
}