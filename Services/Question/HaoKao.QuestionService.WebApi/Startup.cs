using Girvs;
using Girvs.AuthorizePermission.Extensions;
using Girvs.Cache.CacheImps;
using Girvs.DynamicWebApi;
using Girvs.Infrastructure.Extensions;
using Girvs.TypeFinder;
using HaoKao.QuestionService.Application.QuestionHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace HaoKao.QuestionService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : IGirvsStartup
{
    public IConfiguration Configuration => configuration;

    public IWebHostEnvironment WebHostEnvironment => webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
        services.AddControllersWithAuthorizePermissionFilter(options => options.Filters.Add<GirvsModelStateInvalidFilter>());
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);

        var typeFinder = new WebAppTypeFinder();
        var questionTypes = typeFinder.FindOfType<IQuestion>();
        foreach (var t in questionTypes)
        {
            services.AddScoped(typeof(IQuestion), t);
        }
        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
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
        app.ConfigureRequestPipeline(env);

        #if DEBUG
        app.Use(async (ctx, next) =>
        {
            if (!ctx.Request.Path.ToString().ToLower().Contains("manage") && !ctx.Request.Headers.ContainsKey("TenantId"))
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