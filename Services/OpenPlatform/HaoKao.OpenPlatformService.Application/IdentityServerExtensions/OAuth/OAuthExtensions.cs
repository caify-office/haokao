using AspNet.Security.OAuth.Apple;
using AspNet.Security.OAuth.Gitee;
using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.Weixin;
using Girvs.Extensions;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth.HaokaoApple;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth.HaokaoGitee;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth.HaokaoGithub;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth.HaokaoWeixin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;

public static class OAuthExtensions
{
    public static AuthenticationBuilder AddHaoKaoGitee(
        this AuthenticationBuilder builder,
        Action<GiteeAuthenticationOptions> configuration,
        string scheme = GiteeAuthenticationDefaults.AuthenticationScheme,
        string displayName = ""
    )
    {
        if (string.IsNullOrEmpty(displayName)) displayName = GiteeAuthenticationDefaults.DisplayName;
        return builder.AddOAuth<GiteeAuthenticationOptions, HaoKaoGiteeAuthenticationHandler>(scheme, displayName, configuration);
    }

    public static AuthenticationBuilder AddHaoKaoWeixin(
        this AuthenticationBuilder builder,
        Action<WeixinAuthenticationOptions> configuration,
        string scheme = WeixinAuthenticationDefaults.AuthenticationScheme,
        string displayName = ""
    )
    {
        if (string.IsNullOrEmpty(displayName)) displayName = WeixinAuthenticationDefaults.DisplayName;
        return builder.AddOAuth<WeixinAuthenticationOptions, HaoKaoWeixinAuthenticationHandler>(scheme, displayName, configuration);
    }

    public static AuthenticationBuilder AddHaoKaoGitHub(
        this AuthenticationBuilder builder,
        Action<GitHubAuthenticationOptions> configureOptions,
        string authenticationScheme = GitHubAuthenticationDefaults.AuthenticationScheme,
        string displayName = ""
    )
    {
        if (string.IsNullOrEmpty(displayName)) displayName = GitHubAuthenticationDefaults.AuthenticationScheme;
        return builder.AddOAuth<GitHubAuthenticationOptions, HaoKaoGitHubAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }

    public static AuthenticationBuilder AddHaoKaoApple(
        this AuthenticationBuilder builder,
        Action<AppleAuthenticationOptions> configuration,
        string scheme = AppleAuthenticationDefaults.AuthenticationScheme,
        string displayName = ""
    )
    {
        if (displayName.IsNullOrEmpty()) displayName = AppleAuthenticationDefaults.DisplayName;

        builder.Services.AddMemoryCache();
        builder.Services.AddOptions();
        builder.Services.TryAddSingleton<IPostConfigureOptions<AppleAuthenticationOptions>, ApplePostConfigureOptions>();
        return builder.AddOAuth<AppleAuthenticationOptions, HaoKaoAppleAuthenticationHandler>(scheme, displayName, configuration);
    }
}