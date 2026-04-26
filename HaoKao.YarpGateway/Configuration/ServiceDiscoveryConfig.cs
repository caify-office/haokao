using Girvs.Configuration;

namespace HaoKao.YarpGateway.Configuration;

public class ServiceDiscoveryConfig : IAppModuleConfig
{
    public ServiceDiscoveryType ServiceDiscoveryType { get; set; }
    public string ServiceDiscoveryAddress { get; set; } = "http://192.168.51.166:8500";

    public void Init() { }
}

public enum ServiceDiscoveryType
{
    Consul,
    Kubernetes
}
