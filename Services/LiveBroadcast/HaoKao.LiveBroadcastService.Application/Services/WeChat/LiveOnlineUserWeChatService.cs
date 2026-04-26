using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Web;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveOnlineUserWeChatService(ILiveOnlineUserWebService service) : ILiveOnlineUserWeChatService
{
    /// <summary>
    /// 获取直播间在线用户
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    public Task<List<OnlineUserViewModel>> GetOnlineUserList(Guid liveId)
    {
        return service.GetOnlineUserList(liveId);
    }
}