using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;

public class IdentityServerModuleStartup : IAppModuleStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

    }
    public void ConfigureMapEndpointRoute(IEndpointRouteBuilder builder) { }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
    {
        application.UseIdentityServer();
    }

    public int Order => 7;
}