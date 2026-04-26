using Girvs;
using Girvs.JsonConverters;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;

namespace HaoKao.OpenPlatformService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment):IGirvsStartup
{
    public IConfiguration Configuration { get; } = configuration;

    public IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.All);
        // services.AddControllersWithAuthorizePermissionFilter();
        services.AddControllers(options => options.Filters.Add<GirvsModelStateInvalidFilter>())
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter()))
                .AddXmlDataContractSerializerFormatters();

        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddDataProtection();

        services.AddHaoKaoIdentityServer();
        services.AddHaoKaoAuthentication();

        // var signalServiceBuilder = services.AddSignalR();
        // var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        // if (cacheConfig.EnableCaching && cacheConfig.DistributedCacheType == CacheType.Redis)
        // {
        //     signalServiceBuilder.AddStackExchangeRedis(cacheConfig.RedisCacheConfig.ConnectionString);
        // }

        // services.AddSingleton<OnlineDeviceState>();
        // services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtOptions>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCookiePolicy();
        // app.UseStaticFiles();

        app.UseOcelotIdentityServerOrigin();
        app.UseForwardedHeaders();
        app.UseGirvsExceptionHandler();
        app.UseRouting();
        app.UseCors();
        app.ConfigureRequestPipeline(env);
        app.UseEndpoints(endpoints =>
        {
            // endpoints.MapHub<OnlineDeviceHub>("/hubs/onlineDeviceHub");
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
        });
    }
}

// public class ConfigureJwtOptions : IPostConfigureOptions<JwtBearerOptions>
// {
//     public void PostConfigure(string name, JwtBearerOptions options)
//     {
//         var originalOnMessageReceived = options.Events.OnMessageReceived;
//         options.Events.OnMessageReceived = async context =>
//         {
//             await originalOnMessageReceived(context);
//
//             if (string.IsNullOrEmpty(context.Token))
//             {
//                 var accessToken = context.Request.Query["access_token"];
//                 var path = context.HttpContext.Request.Path;
//                 if (!string.IsNullOrEmpty(accessToken) &&
//                     path.StartsWithSegments("/hubs"))
//                 {
//                     context.Token = accessToken;
//                 }
//             }
//         };
//     }
// }