using HaoKao.OpenPlatformService.Domain.Entities;
using IdentityModel;
using IdentityServer4;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;

public static class RegisterUserExtensions
{
    public static IEnumerable<System.Security.Claims.Claim> BuildClaims(this RegisterUser user)
    {
        return user.ToIdentityServerUser().CreatePrincipal().Claims;
    }

    public static IdentityServerUser ToIdentityServerUser(this RegisterUser user)
    {
        var identityServerUser = new IdentityServerUser(user.Id.ToString())
        {
            DisplayName = user.NickName,
            AuthenticationTime = DateTime.UtcNow,
            AdditionalClaims =
            [
                new(GirvsIdentityClaimTypes.UserId, user.Id.ToString()),
                new(GirvsIdentityClaimTypes.UserName, user.NickName ?? string.Empty),
                new(GirvsIdentityClaimTypes.IdentityType, IdentityType.RegisterUser.ToString()),
                new(GirvsIdentityClaimTypes.ClaimSystemModule, SystemModule.All.ToString()),
                new(JwtClaimTypes.NickName, user.NickName ?? string.Empty),
                new(JwtClaimTypes.Name, user.NickName ?? string.Empty),
                new(JwtClaimTypes.PhoneNumber, user.Phone ?? string.Empty),
                new(JwtClaimTypes.Email, user.EmailAddress ?? string.Empty),
                new(JwtClaimTypes.StateHash, user.UserState.ToString()),
                new(JwtClaimTypes.Gender, user.UserGender.ToString()),
                new(JwtClaimTypes.Picture, user.HeadImage ?? string.Empty)
            ]
        };
        return identityServerUser;
    }
}