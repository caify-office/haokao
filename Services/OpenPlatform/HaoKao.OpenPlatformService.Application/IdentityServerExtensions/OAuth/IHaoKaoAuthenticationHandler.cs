namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;

public interface IHaoKaoAuthenticationHandler
{
    string GetRedirectUrl(AuthenticationProperties properties, string authenticationCallBackUrl);

    ExternalUser GetCurrentAuthenticationUniqueIdentifier(IEnumerable<System.Security.Claims.Claim> claims);

    Task<ExternalUser> CreateExternalTicketAsync(ExternalCodeAuthenticationOptions options);
}