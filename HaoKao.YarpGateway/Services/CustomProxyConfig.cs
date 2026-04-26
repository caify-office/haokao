using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading;
using Yarp.ReverseProxy.Configuration;

namespace HaoKao.YarpGateway.Services;

// YARP 代理配置类
public class CustomProxyConfig(
    IReadOnlyList<RouteConfig> routes,
    IReadOnlyList<ClusterConfig> clusters
) : IProxyConfig
{
    public IReadOnlyList<RouteConfig> Routes { get; } = routes;
    public IReadOnlyList<ClusterConfig> Clusters { get; } = clusters;

    public IChangeToken ChangeToken => new CancellationChangeToken(_cts.Token);

    private readonly CancellationTokenSource _cts = new();

    public IChangeToken GetChangeToken() => new CancellationChangeToken(_cts.Token);
}
