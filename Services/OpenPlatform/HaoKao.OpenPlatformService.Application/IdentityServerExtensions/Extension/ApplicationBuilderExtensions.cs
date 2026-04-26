using Girvs.Configuration;
using HaoKao.OpenPlatformService.Application.Configuration;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseOcelotIdentityServerOrigin(this IApplicationBuilder app)
    {
        var appSettings = Singleton<AppSettings>.Instance;


        if (appSettings.ModuleConfigurations[nameof(ExternalPlatformConfig)] is not ExternalPlatformConfig externalPlatformConfig)
        {
            throw new GirvsException("未找到对应的配置");
        }

        if (!string.IsNullOrEmpty(externalPlatformConfig.ExternalAuthenticationProxyUrl))
        {
            app.Use(async (context, next) =>
            {
                if (!(context.Request.Path.HasValue
                   && context.Request.Path.Value.ToLower().IndexOf("/api/registeruserservice") == 0)
                  )
                {
                    context.SetIdentityServerOrigin(externalPlatformConfig.ExternalAuthenticationProxyUrl);
                }
                await next();
            });
        }

        return app;
    }
}