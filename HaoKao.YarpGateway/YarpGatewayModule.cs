using Consul;
using Girvs.Configuration;
using Girvs.Infrastructure;
using HaoKao.YarpGateway.Configuration;
using HaoKao.YarpGateway.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;
using ConsulClient = Consul.ConsulClient;

namespace HaoKao.YarpGateway;

public class YarpGatewayModule : IAppModuleStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var config = Singleton<AppSettings>.Instance.Get<ServiceDiscoveryConfig>();
        if (config.ServiceDiscoveryType == ServiceDiscoveryType.Consul)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(config.ServiceDiscoveryAddress);
            }));
        }

        services.AddSingleton<IProxyConfigProvider, CustomProxyConfigProvider>();

        // 添加 YARP 反向代理
        services
            .AddReverseProxy()
            .AddTransforms(context =>
            {
                context.AddOriginalHost(false);
                context.CopyRequestHeaders = true;
                context.AddXForwarded(ForwardedTransformActions.Append);
                context.AddXForwardedFor(
                    headerName: "X-Forwarded-For",
                    ForwardedTransformActions.Append
                );
            });
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment env) { }

    public void ConfigureMapEndpointRoute(IEndpointRouteBuilder builder)
    {
        builder.MapReverseProxy();
    }

    public int Order { get; } = 100000;
}
