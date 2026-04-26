using Girvs.Configuration;
using HaoKao.YarpGateway.Configuration;

namespace HaoKao.YarpGateway;

public class YarpGatewayConfig : IAppModuleConfig
{
    public ServiceDiscoveryType ServiceDiscoveryType { get; set; }

    public string ServiceDiscoveryAddress { get; set; } = "http://192.168.51.166:8500";

    public void Init() { }
}

