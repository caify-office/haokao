using Microsoft.AspNetCore.Hosting;

namespace HaoKao.Common;

public class HaoKaoFanAppModule : IAppModuleStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration) { }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
    {
        application.InitAuthorizeData();
    }

    public void ConfigureMapEndpointRoute(IEndpointRouteBuilder builder) { }

    public int Order { get; } = int.MaxValue;

}