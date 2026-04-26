using Girvs.AuthorizePermission.Extensions;
using Girvs.DynamicWebApi;
using Girvs.Infrastructure;
using Girvs.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Linq;
using Girvs;

namespace HaoKao.LearningPlanService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment):IGirvsStartup
{
    public IConfiguration Configuration { get; } = configuration;

    public IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithAuthorizePermissionFilter(options =>
                                                                 options.Filters.Add<GirvsModelStateInvalidFilter>());
        services.AddControllersWithAuthorizePermissionFilter();
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseGirvsExceptionHandler();
        app.UseRouting();
        app.ConfigureRequestPipeline(env);
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.HasValue
             && context.Request.Path.Value.ToLower().IndexOf("/api/website/lecturerwebservice") == 0
               )
            {
                //�����ǰû�е�½,����Ϊ��ʱ�û�
                if (!context.User.Identity.IsAuthenticated)
                {
                    var sid = Guid.NewGuid();
                    var examinee = "�ο�";

                    var girvsIdentityClaim = new GirvsIdentityClaim
                    {
                        UserId = sid.ToString(),
                        UserName = examinee,
                        TenantId = EngineContext.Current.HttpContext.Request.Headers[nameof(GirvsIdentityClaim.TenantId)],
                        SystemModule = (SystemModule)0,
                        IdentityType = IdentityType.RegisterUser,
                        OtherClaims = new Dictionary<string, string>()
                    };
                    var claims = EngineContext.Current.ClaimManager.BuildClaimsIdentity(girvsIdentityClaim);

                    EngineContext.Current.ClaimManager.SetFromDictionary(claims.Claims.ToDictionary(x => x.Type, x => x.Value));
                }
            }

            await next(); //������һ���м��
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
        });
    }
}