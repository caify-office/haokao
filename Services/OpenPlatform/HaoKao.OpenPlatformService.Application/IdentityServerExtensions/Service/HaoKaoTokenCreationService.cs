using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Service;

public class HaoKaoTokenCreationService(
#pragma warning disable CS0618 // 类型或成员已过时
    ISystemClock clock,
#pragma warning restore CS0618 // 类型或成员已过时
    IKeyMaterialService keys,
    IdentityServerOptions options,
    ILogger<DefaultTokenCreationService> logger
) : DefaultTokenCreationService(clock, keys, options, logger)
{
    public override async Task<string> CreateTokenAsync(Token token)
    {
        var accessToken = await base.CreateTokenAsync(token);

        return accessToken;
    }
}