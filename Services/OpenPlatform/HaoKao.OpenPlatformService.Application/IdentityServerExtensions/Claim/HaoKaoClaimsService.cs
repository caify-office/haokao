using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Claim;

public class HaoKaoClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : DefaultClaimsService(profile, logger)
{
    public override Task<IEnumerable<System.Security.Claims.Claim>> GetIdentityTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resources, bool includeAllIdentityClaims, ValidatedRequest request)
    {
        return base.GetIdentityTokenClaimsAsync(subject, resources, includeAllIdentityClaims, request);
    }

    public override async Task<IEnumerable<System.Security.Claims.Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resourceResult, ValidatedRequest request)
    {
        var result = (await base.GetAccessTokenClaimsAsync(subject, resourceResult, request)).ToList();

        foreach (var claim in subject.Claims)
        {
            if (claim.Type == GirvsIdentityClaimTypes.UserId)
            {
                result.Add(claim);
            }

            if (claim.Type == GirvsIdentityClaimTypes.UserName)
            {
                result.Add(claim);
            }

            if (claim.Type == GirvsIdentityClaimTypes.IdentityType)
            {
                result.Add(claim);
            }

            if (claim.Type == GirvsIdentityClaimTypes.ClaimSystemModule)
            {
                result.Add(claim);
            }
        }

        return result;
    }
}