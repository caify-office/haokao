using AspNet.Security.OAuth.Gitee;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth.HaokaoGitee;

public class HaoKaoGiteeAuthenticationHandler(
    IOptionsMonitor<GiteeAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : GiteeAuthenticationHandler(options, logger, encoder), IHaoKaoAuthenticationHandler
{
    public string GetRedirectUrl(AuthenticationProperties properties, string authenticationCallBackUrl)
    {
        var redirectUri = string.IsNullOrEmpty(authenticationCallBackUrl)
            ? BuildRedirectUri(Options.CallbackPath)
            : authenticationCallBackUrl + Options.CallbackPath;

        GenerateCorrelationId(properties);
        return base.BuildChallengeUrl(properties, redirectUri);
    }

    public ExternalUser GetCurrentAuthenticationUniqueIdentifier(IEnumerable<System.Security.Claims.Claim> claims)
    {
        var nameIdentifier = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var nikeName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var other = claims.ToDictionary(x => x.Type, x => x.Value);
        return new ExternalUser
        {
            Schemem = GiteeAuthenticationDefaults.AuthenticationScheme,
            NikeName = nikeName,
            UniqueIdentifier = nameIdentifier,
            OtherInformation = other
        };
    }

    public Task<ExternalUser> CreateExternalTicketAsync(ExternalCodeAuthenticationOptions options)
    {
        throw new NotImplementedException();
    }
}