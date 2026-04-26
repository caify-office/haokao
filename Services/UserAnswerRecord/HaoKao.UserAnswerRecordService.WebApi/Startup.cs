using Girvs;
using Girvs.AuthorizePermission.Extensions;
using Girvs.DynamicWebApi;
using Girvs.Infrastructure.Extensions;
using HaoKao.UserAnswerRecordService.Application.Worker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.UserAnswerRecordService.WebApi;

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

        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            if (!int.TryParse(Configuration["QueueCapacity"], out var queueCapacity))
            {
                queueCapacity = 100;
            }
            return new BackgroundTaskQueue(queueCapacity);
        });
        services.AddHostedService<QueuedHostedService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseGirvsExceptionHandler();
        app.UseRouting();

        #if DEBUG
        app.Use(async (ctx, next) =>
        {
            if (!ctx.Request.Path.ToString().ToLower().Contains("management") && !ctx.Request.Headers.ContainsKey("TenantId"))
            {
                ctx.Request.Headers["TenantId"] = "08db5bf2-afae-4d40-8896-18e7e86b6b37";
            }
            await next();
        });
        #endif

        app.ConfigureRequestPipeline(env);
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
        });
    }
}