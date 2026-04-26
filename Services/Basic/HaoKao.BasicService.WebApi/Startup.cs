using Girvs;
using Girvs.AuthorizePermission.Extensions;
using Girvs.DynamicWebApi;
using Girvs.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.BasicService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : IGirvsStartup
{
    private IConfiguration Configuration { get; } = configuration;

    private IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithAuthorizePermissionFilter(options => options.Filters.Add<GirvsModelStateInvalidFilter>());
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
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