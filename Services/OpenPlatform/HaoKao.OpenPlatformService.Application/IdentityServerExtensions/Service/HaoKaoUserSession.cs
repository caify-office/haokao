using System.Net;
using Girvs.Extensions;
using HaoKao.OpenPlatformService.Domain.Repositories;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Service;

public class HaoKaoUserSession(
    IHttpContextAccessor httpContextAccessor,
    IAuthenticationHandlerProvider handlers,
    IdentityServerOptions options,
#pragma warning disable CS0618 // 类型或成员已过时
    ISystemClock clock,
#pragma warning restore CS0618 // 类型或成员已过时
    ILogger<IUserSession> logger,
    IPersistedGrantRepository repository
) : DefaultUserSession(httpContextAccessor, handlers, options, clock, logger)
{
    private readonly IPersistedGrantRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<IUserSession> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override async Task<string> CreateSessionIdAsync(ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        var sessionId = await base.CreateSessionIdAsync(principal, properties);

        try
        {
            var uri = new Uri(WebUtility.UrlDecode(properties.RedirectUri));
            var query = QueryHelpers.ParseQuery(uri.Query);
            var userId = principal.Claims.FirstOrDefault(x => x.Type == GirvsIdentityClaimTypes.UserId)?.Value;
            var clientId = query["client_id"];

            var persistedGrant = new Domain.Entities.PersistedGrant
            {
                Key = (userId + clientId + sessionId).ToMd5(),
                Type = query["response_type"],
                SubjectId = userId,
                ClientId = clientId,
                SessionId = sessionId,
                CreationTime = DateTime.Now,
                Expiration = properties?.ExpiresUtc?.UtcDateTime,
                Data = JsonConvert.SerializeObject(principal.Claims.Select(x => new { x.Type, x.Value }))
            };
            await _repository.AddAsync(persistedGrant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateSessionIdAsync));
        }

        return sessionId;
    }

    public override CookieOptions CreateSessionIdCookieOptions()
    {
        return base.CreateSessionIdCookieOptions();
    }

    public override Task EnsureSessionIdCookieAsync()
    {
        return base.EnsureSessionIdCookieAsync();
    }

    public override Task<string> GetSessionIdAsync()
    {
        return base.GetSessionIdAsync();
    }

    public override void IssueSessionIdCookie(string sid)
    {
        base.IssueSessionIdCookie(sid);
    }

    public override Task RemoveSessionIdCookieAsync()
    {
        return base.RemoveSessionIdCookieAsync();
    }

    public override Task AddClientIdAsync(string clientId)
    {
        return base.AddClientIdAsync(clientId);
    }

    public override Task<IEnumerable<string>> GetClientListAsync()
    {
        return base.GetClientListAsync();
    }

    public override Task<ClaimsPrincipal> GetUserAsync()
    {
        return base.GetUserAsync();
    }
}