using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Web;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 直播管理员接口服务-微信小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveAdministratorWeChatService(
    ILiveAdministratorWebService liveAdministratorWebService) : ILiveAdministratorWeChatService
{
    #region 初始参数

    private readonly ILiveAdministratorWebService _liveAdministratorWebService = liveAdministratorWebService ?? throw new ArgumentNullException(nameof(liveAdministratorWebService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 是否是直播管理员
    /// </summary>
    [HttpGet]
    public Task<bool> IsLiveAdmin()
    {
        return _liveAdministratorWebService.IsLiveAdmin();
    }

    #endregion
}