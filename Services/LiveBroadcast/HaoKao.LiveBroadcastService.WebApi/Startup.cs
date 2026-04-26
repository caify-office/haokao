using Girvs;
using Girvs.AuthorizePermission.Extensions;
using Girvs.Cache.Configuration;
using Girvs.DynamicWebApi;
using Girvs.Infrastructure;
using Girvs.Infrastructure.Extensions;
using HaoKao.LiveBroadcastService.Application.Hubs;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using HaoKao.LiveBroadcastService.Application.Workers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace HaoKao.LiveBroadcastService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment):IGirvsStartup
{
    public IConfiguration Configuration { get; } = configuration;

    public IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
        services.AddControllersWithAuthorizePermissionFilter(options => options.Filters.Add<GirvsModelStateInvalidFilter>());
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);

        // TODO 8.0 版本记得删除下面的代码
        var signalServiceBuilder = services.AddSignalR(o => o.AddFilter<LiveChatHubFilter>()).AddMessagePackProtocol();
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        if (cacheConfig.EnableCaching && cacheConfig.DistributedCacheConfig.DistributedCacheType == DistributedCacheType.Redis)
        {
            signalServiceBuilder.AddStackExchangeRedis(cacheConfig.DistributedCacheConfig.ConnectionString);
        }

        services.AddSingleton<OnlineUserState>();
        services.AddSingleton<ILiveMessageQueue, LiveMessageQueue>();
        services.AddHostedService<LiveMessageHostedService>();

        services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtOptions>());
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureTenantOptions>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        });

        app.UseStaticFiles();

        app.UseGirvsExceptionHandler();
        app.UseRouting();
        app.ConfigureRequestPipeline(env);
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<LiveChatHub>("/hubs/liveChatHub");
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
        });
    }
}

public class ConfigureJwtOptions : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string name, JwtBearerOptions options)
    {
        options.Events ??= new JwtBearerEvents();
        var originalOnMessageReceived = options.Events.OnMessageReceived;
        options.Events.OnMessageReceived = async context =>
        {
            await originalOnMessageReceived(context);

            if (string.IsNullOrEmpty(context.Token))
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
            }
        };
    }
}

public class ConfigureTenantOptions : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string name, JwtBearerOptions options)
    {
        options.Events ??= new JwtBearerEvents();
        var originalOnMessageReceived = options.Events.OnMessageReceived;
        options.Events.OnMessageReceived = async context =>
        {
            await originalOnMessageReceived(context);

            var path = context.HttpContext.Request.Path;
            if (path.StartsWithSegments("/hubs"))
            {
                var tenantId = context.Request.Query["tenantId"];
                var tenantName = context.Request.Query["tenantName"];
                EngineContext.Current.HttpContext.Request.Headers["TenantId"] = tenantId;
                EngineContext.Current.HttpContext.Request.Headers["TenantName"] = tenantName;
                EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
                {
                    { GirvsIdentityClaimTypes.TenantId, tenantId },
                    { GirvsIdentityClaimTypes.TenantName, tenantName },
                });
            }
        };
    }
}