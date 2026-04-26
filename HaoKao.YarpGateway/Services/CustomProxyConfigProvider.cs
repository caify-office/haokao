using Girvs.Infrastructure;
using HaoKao.YarpGateway.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using Yarp.ReverseProxy.Configuration;
using RouteConfig = Yarp.ReverseProxy.Configuration.RouteConfig;

namespace HaoKao.YarpGateway.Services;

public class CustomProxyConfigProvider : IProxyConfigProvider
{
    private volatile CustomProxyConfig _config;
    private Timer _updateTimer;
    private CancellationTokenSource _cts = new();

    private IList<ClientServiceConfig> GetServices()
    {
        var config = EngineContext.Current.GetAppModuleConfig<ServiceDiscoveryConfig>();
        if (config.ServiceDiscoveryType== ServiceDiscoveryType.Consul)
        {
            return ConsulClientService.GetConsulClientServices();
        }
        else
        {
            return KubernetesClientService.GetKubernetesClientServices();
        }
    }

    public CustomProxyConfigProvider()
    {
        _config = new CustomProxyConfig(new List<RouteConfig>(), new List<ClusterConfig>());
        var services = GetServices();
        UpdateConfig(services);
        // 定期从 Consul 获取最新服务列表
        _updateTimer = new Timer(
            _ => UpdateConfig(GetServices()),
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(30)
        );
    }

    public IProxyConfig GetConfig() => _config;

    private void UpdateConfig(IList<ClientServiceConfig> services)
    {
        try
        {
            var clusters = new List<ClusterConfig>();
            var routes = new List<RouteConfig>();

            foreach (var agentService in services)
            {
                var serverName = agentService.ServiceName;
                if (routes.Exists(x => x.RouteId == serverName))
                    continue;

                routes.Add(
                    new RouteConfig
                    {
                        RouteId = serverName,
                        ClusterId = serverName,
                        Match = new RouteMatch { Path = $"/{serverName}/" + "{**catch-all}" },
                        Transforms = new List<Dictionary<string, string>>
                        {
                            new() { { "PathRemovePrefix", serverName }, }
                        }
                    }
                );

                clusters.Add(
                    new ClusterConfig
                    {
                        ClusterId = serverName,
                        Destinations = agentService.Destinations
                    }
                );
            }

            // 更新配置
            _config = new CustomProxyConfig(routes, clusters);
            _cts.Cancel(); // 通知 YARP 配置已更新
            _cts = new CancellationTokenSource();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Failed to update services from Consul: {ex.Message}");
        }
    }
}
