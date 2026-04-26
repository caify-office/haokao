using Girvs;
using HaoKao.DataStatisticsService.WebApi.Services;

namespace HaoKao.DataStatisticsService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment):IGirvsStartup
{
    public IConfiguration Configuration => configuration;

    public IWebHostEnvironment WebHostEnvironment => webHostEnvironment;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithAuthorizePermissionFilter(options => options.Filters.Add<GirvsModelStateInvalidFilter>());
        services.AddControllersWithAuthorizePermissionFilter();
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);
        services.AddScoped<ProgressStatisticsService>();
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