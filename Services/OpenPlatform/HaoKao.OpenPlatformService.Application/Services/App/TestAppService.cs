using Girvs.AuthorizePermission;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.App;

[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class TestAppService : ITestAppService
{
    public Task<string> Get()
    {
        return Task.FromResult("Authentication 成功");
    }
}