using Girvs.AuthorizePermission.Extensions;
using Girvs.DynamicWebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Girvs.Infrastructure.Extensions;
using Girvs.TypeFinder;
using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Domain.Statistics;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json;
using System.Text.Json.Serialization;
using Girvs;

namespace HaoKao.OrderService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment):IGirvsStartup
{
    public IConfiguration Configuration { get; } = configuration;

    public IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        services.AddControllersWithAuthorizePermissionFilter(options => options.Filters.Add<GirvsModelStateInvalidFilter>());
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);

        var typeFinder = new WebAppTypeFinder();

        var types = typeFinder.FindOfType<IPayHandler>();
        foreach (var t in types)
        {
            services.AddScoped(typeof(IPayHandler), t);
        }

        types = typeFinder.FindOfType<ISalesStatistics>();
        foreach (var t in types)
        {
            services.AddScoped(typeof(ISalesStatistics), t);
        }

        services.PostConfigure<JsonSerializerOptions>(options =>
        {
            options.ReferenceHandler = ReferenceHandler.Preserve;
            options.WriteIndented = true;
        });
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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
        });
    }
}