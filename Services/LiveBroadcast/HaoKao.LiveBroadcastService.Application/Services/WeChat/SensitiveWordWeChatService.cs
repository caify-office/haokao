using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Web;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class SensitiveWordWeChatService(ISensitiveWordWebService webService) : ISensitiveWordWeChatService
{
    [HttpGet]
    public Task<BrowseSensitiveWordViewModel> Get()
    {
        return webService.Get();
    }
}