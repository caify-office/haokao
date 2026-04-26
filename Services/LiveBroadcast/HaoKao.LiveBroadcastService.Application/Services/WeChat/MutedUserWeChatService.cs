using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 禁言用户接口-微信小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class MutedUserWeChatService(IMutedUserService service) : IMutedUserWeChatService
{
    private readonly IMutedUserService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 是否被禁言
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<bool> IsMuted()
    {
        return _service.IsMuted();
    }

    /// <summary>
    /// 禁言用户
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public Task Mute([FromBody] CreateMutedUserViewModel viewModel)
    {
        return _service.Create(viewModel);
    }
}