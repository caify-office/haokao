using Girvs.Configuration;
using HaoKao.OpenPlatformService.Application.Configuration;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;

public static class ServiceCollectionExtensions
{
    public static AuthenticationBuilder AddHaoKaoAuthentication(this IServiceCollection services)
    {
        var appSettings = Singleton<AppSettings>.Instance;

        if (appSettings.ModuleConfigurations[nameof(ExternalPlatformConfig)] is not ExternalPlatformConfig externalPlatformConfig)
        {
            throw new GirvsException("未找到对应的配置");
        }

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        // 这里配置会影响 .AspNetCore.Cookies
        var builder = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                              .AddCookie(options =>
                              {
                                  // Implicit Flow 的 SigninSilent 需要把Cookie设置为 SameSite-None
                                  // 否则前端通过iFrame发起令牌刷新请求会被浏览器拦截Cookie导致出现login_required的错误信息
                                  options.Cookie.SameSite = SameSiteMode.None;
                                  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                                  options.Cookie.IsEssential = true;
                                  options.Cookie.HttpOnly = false;
                                  // 设置 Cookie 共享域
                                  options.Cookie.Domain = ".haokao123.com";
                                  // options.Events.OnRedirectToReturnUrl = async context =>
                                  // {
                                  //     if (IsAjaxRequest(context.Request))
                                  //     {
                                  //         context.Response.StatusCode = (int)HttpStatusCode.OK;
                                  //         await context.Response.WriteAsync(context.RedirectUri);
                                  //     }
                                  //     else
                                  //     {
                                  //         context.Response.Redirect(context.RedirectUri);
                                  //     }
                                  // };
                              });

        if (externalPlatformConfig.WeixinAuthenticationOptions.Enable)
        {
            builder.AddHaoKaoWeixin(options =>
            {
                //options.AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
                ActionOptions(options, externalPlatformConfig.WeixinAuthenticationOptions);
            });
        }

        if (externalPlatformConfig.GiteeAuthenticationOptions.Enable)
        {
            builder.AddHaoKaoGitee(options => { ActionOptions(options, externalPlatformConfig.GiteeAuthenticationOptions); });
        }

        if (externalPlatformConfig.GitHubAuthenticationOptions.Enable)
        {
            builder.AddHaoKaoGitHub(options => { ActionOptions(options, externalPlatformConfig.GitHubAuthenticationOptions); });
        }

        if (externalPlatformConfig.AppleAuthenticationOptions.Enable)
        {
            builder.AddHaoKaoApple(options =>
            {
                var config = externalPlatformConfig.AppleAuthenticationOptions;
                ActionOptions(options, config);
                options.KeyId = config.KeyId;
                options.TeamId = config.TeamId;
                options.UsePrivateKey(_ => CommonHelper.DefaultFileProvider.GetFileInfo(config.PrviateKeyFilePath));
            });
        }

        return builder;
    }

    private static void ActionOptions(OAuthOptions options, ExternalPlatformConfig.HaoKaoAuthenticationOptions config)
    {
        var defaultCallbackPath = options.CallbackPath;
        var callbackPath = new PathString($"{config.CallbackPathPrefix}{defaultCallbackPath}");

        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.ClientId = config.ClientId;
        options.ClientSecret = config.ClientSecret;
        options.CallbackPath = callbackPath;
        options.CorrelationCookie.SecurePolicy = config.CookieSecurePolicy;
        options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
        options.CorrelationCookie.HttpOnly = false;
        options.CorrelationCookie.Path = "/";
    }

    // private static bool IsAjaxRequest(HttpRequest request)
    // {
    //     return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
    //            string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
    // }
}