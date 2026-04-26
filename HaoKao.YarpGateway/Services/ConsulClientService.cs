using Consul;
using Girvs.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using DestinationConfig = Yarp.ReverseProxy.Configuration.DestinationConfig;

namespace HaoKao.YarpGateway.Services;

public class ConsulClientService
{
    public static List<ClientServiceConfig> GetConsulClientServices()
    {
        var consulClient = EngineContext.Current.Resolve<IConsulClient>();

        var servers = consulClient.Agent.Services().Result.Response;

        var result = new List<ClientServiceConfig>();

        foreach (var agentService in servers)
        {
            if (result.Any(x => x.ServiceName == agentService.Value.Service))
                continue;

            var serviceName = agentService.Value.Service.Replace("-", "_");
            var address = agentService.Value.Address;
            var port = agentService.Value.Port;

            result.Add(
                new ClientServiceConfig
                {
                    ServiceName = serviceName,
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            $"{serviceName}-{port}",
                            new DestinationConfig { Address = $"http://{address}:{port}" }
                        }
                    }
                }
            );
        }

        return result;
    }
}
