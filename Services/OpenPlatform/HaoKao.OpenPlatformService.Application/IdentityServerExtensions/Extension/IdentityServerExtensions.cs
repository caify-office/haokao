using Girvs.Configuration;
using HaoKao.OpenPlatformService.Application.Configuration;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Claim;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Service;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Store;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Validator;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;

public static class IdentityServerExtensions
{
    public static IIdentityServerBuilder AddHaoKaoIdentityServer(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.Secure = CookieSecurePolicy.Always; // 第三方登录时 SameSite-None 必须带有 secure 属性, 否则会被浏览器拦截Cookie导致服务器500的异常
            options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });

        //设置跨域
        services.AddCors(x => x.AddDefaultPolicy(policy => policy.SetIsOriginAllowed(_ => true)
                                                                 .WithOrigins("https://*.haokao123.com")
                                                                 .SetIsOriginAllowedToAllowWildcardSubdomains()
                                                                 .AllowAnyMethod()
                                                                 .AllowAnyHeader()
                                                                 .AllowCredentials()));

        services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
            options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
            options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
        });

        var builder = services.AddIdentityServer(options =>
        {
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.MutualTls.Enabled = false;
            options.MutualTls.DomainName = "mtls";
            options.Cors.CorsPaths.Add(new PathString("/connect/authorize"));
            options.Cors.CorsPaths.Add(new PathString("/connect/authorize/callback"));
            options.Cors.CorsPaths.Add(new PathString("/api/AccountService/login"));
            options.Cors.CorsPaths.Add(new PathString("/api/AccountService/logout"));

            // options.Csp.Level =  CspLevel.One;
            // options.Csp.AddDeprecatedHeader = false;

            if (Singleton<AppSettings>.Instance.ModuleConfigurations[nameof(ExternalPlatformConfig)]
                is ExternalPlatformConfig externalPlatformConfig)
            {
                options.UserInteraction.LogoutUrl = externalPlatformConfig.ClientLogoutUrl;
                options.UserInteraction.LoginUrl = externalPlatformConfig.ClientLoginUrl;
                options.UserInteraction.ErrorUrl = externalPlatformConfig.ClientErrorUrl;
                options.UserInteraction.ConsentUrl = externalPlatformConfig.ClientConsentUrl;
                options.UserInteraction.DeviceVerificationUrl = externalPlatformConfig.ClientDeviceVerificationUrl;
            }

            // 这里配置会影响 idsrv.session
            options.Authentication.CookieLifetime = TimeSpan.FromDays(30);
            options.Authentication.CookieSlidingExpiration = true;
            options.Authentication.CookieSameSiteMode = SameSiteMode.None;
            options.Authentication.CheckSessionCookieDomain = ".haokao123.com";
        });

        builder.AddResourceStore<HaoKaoResourcesStore>();
        builder.AddClientStore<HaoKaoClientStore>();
        builder.AddResourceOwnerValidator<HaoKaoResourceOwnerPasswordValidator>();
        builder.AddProfileService<HaoKaoProfileService>();
        // builder.AddPersistedGrantStore<HaoKaoPersistedGrantStore>();

        // 不建议用于生产 - 您需要将密钥材料存储在安全的地方
        // builder.AddDeveloperSigningCredential(filename: "haokaoauthentication.rsa");

        var pathFile = Path.Combine(Directory.GetCurrentDirectory(), "Certificates", "IS4.pfx");
        var x509 = new X509Certificate2(pathFile, "zhuofan168");
        builder.AddSigningCredential(x509);

        builder.Services.AddTransient<IClaimsService, HaoKaoClaimsService>();
        builder.Services.AddTransient<ITokenService, HaoKaoTokenService>();
        builder.Services.AddTransient<IUserSession, HaoKaoUserSession>();
        return builder;
    }

    private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
    {
        if (options.SameSite == SameSiteMode.None)
        {
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            if (MyUserAgentDetectionLib.DisallowsSameSiteNone(userAgent))
            {
                options.SameSite = SameSiteMode.Unspecified;
            }
        }
    }
}