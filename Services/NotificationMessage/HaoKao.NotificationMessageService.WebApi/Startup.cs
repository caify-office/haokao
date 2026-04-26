using Girvs;
using Girvs.AuthorizePermission.Extensions;
using Girvs.DynamicWebApi;
using Girvs.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.NotificationMessageService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment):IGirvsStartup
{
    private IConfiguration Configuration { get; } = configuration;

    private IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        services.AddControllersWithAuthorizePermissionFilter(options =>
                                                                 options.Filters.Add<GirvsModelStateInvalidFilter>());
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);
        #region cap事件调试时使用
        //            services.AddCap(x =>
        //            {
        //                //x.UseGrivsConfigDataBase();
        //                x.UseDashboard(d =>
        //                {
        //#if DEBUG
        //                    //d.PathBase = "/cap";
        //#else
        //                    var virticalPath = System.AppDomain.CurrentDomain.FriendlyName.Replace(".", "_");
        //                    d.PathBase = $"/{virticalPath}";
        //#endif
        //                });
        //            }).AddSubscribeFilter<GirvsCapFilter>();
        #endregion
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