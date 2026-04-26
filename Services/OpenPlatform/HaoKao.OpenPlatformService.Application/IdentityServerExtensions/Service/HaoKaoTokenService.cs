using HaoKao.OpenPlatformService.Domain.Repositories;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Service;

public class HaoKaoTokenService(
    IClaimsService claimsProvider,
    IReferenceTokenStore referenceTokenStore,
    ITokenCreationService creationService,
    IHttpContextAccessor contextAccessor,
#pragma warning disable CS0618 // 类型或成员已过时
    ISystemClock clock,
#pragma warning restore CS0618 // 类型或成员已过时
    IKeyMaterialService keyMaterialService,
    IdentityServerOptions options,
    ILogger<HaoKaoTokenService> logger,
    IPersistedGrantRepository repository,
    IAuthenticationSchemeProvider authenticationSchemeProvider
) : DefaultTokenService(claimsProvider, referenceTokenStore, creationService, contextAccessor, clock, keyMaterialService, options, logger)
{
    private readonly HttpContext _httpContext = contextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(contextAccessor));
    private readonly IPersistedGrantRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider = authenticationSchemeProvider ?? throw new ArgumentNullException(nameof(authenticationSchemeProvider));

    public override async Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
    {
        var token = await base.CreateAccessTokenAsync(request);

        var grants = await _repository.GetBySubjectIdAndClientId(token.SubjectId, token.ClientId);
        if (grants.Count == 0) return token;

        if (!grants.Any(x => x.SessionId == token.SessionId)) return token;

        var allowedSessionId = grants.MaxBy(x => x.CreationTime).SessionId;
        if (token.SessionId == allowedSessionId) return token;

        await _repository.ConsumeAsync(token.SubjectId, token.ClientId, token.SessionId);
        await _httpContext.SignOutAsync(await GetCookieAuthenticationSchemeAsync());

        return token;
    }

    private async Task<string> GetCookieAuthenticationSchemeAsync()
    {
        if (Options.Authentication.CookieAuthenticationScheme != null)
        {
            return Options.Authentication.CookieAuthenticationScheme;
        }

        var scheme = await _authenticationSchemeProvider.GetDefaultAuthenticateSchemeAsync()
        ?? throw new InvalidOperationException("No DefaultAuthenticateScheme found or no CookieAuthenticationScheme configured on IdentityServerOptions.");
        return scheme.Name;
    }

    public override Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
    {
        return base.CreateIdentityTokenAsync(request);
    }

    public override Task<string> CreateSecurityTokenAsync(Token token)
    {
        return base.CreateSecurityTokenAsync(token);
    }
}