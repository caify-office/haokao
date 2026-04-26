using AspNet.Security.OAuth.Apple;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth.HaokaoApple;

public class HaoKaoAppleAuthenticationHandler(
    IOptionsMonitor<AppleAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AppleAuthenticationHandler(options, logger, encoder), IHaoKaoAuthenticationHandler
{
    public string GetRedirectUrl(AuthenticationProperties properties, string authenticationCallBackUrl)
    {
        // Options.Scope.Add("fullName");
        // Options.Scope.Add("userid");
        var redirectUri = string.IsNullOrEmpty(authenticationCallBackUrl)
            ? BuildRedirectUri(Options.CallbackPath)
            : authenticationCallBackUrl + Options.CallbackPath;

        GenerateCorrelationId(properties);
        return BuildChallengeUrl(properties, redirectUri);
    }

    public ExternalUser GetCurrentAuthenticationUniqueIdentifier(IEnumerable<System.Security.Claims.Claim> claims)
    {
        var uniqueIdentifier = claims.FirstOrDefault(x => x.Type == "sub")?.Value;


        var nikeName = claims.FirstOrDefault(x => x.Type == "email")?.Value;
        var otherInformation = claims.ToDictionary(x => x.Type, x => x.Value);
        var email = claims.FirstOrDefault(x => x.Type == "email")?.Value;

        return new ExternalUser
        {
            Schemem = AppleAuthenticationDefaults.AuthenticationScheme,
            NikeName = nikeName,
            EmailAddress = email,
            UniqueIdentifier = uniqueIdentifier,
            OtherInformation = otherInformation
        };
    }

    public Task<ExternalUser> CreateExternalTicketAsync(ExternalCodeAuthenticationOptions options)
    {
        throw new NotImplementedException();
    }
}