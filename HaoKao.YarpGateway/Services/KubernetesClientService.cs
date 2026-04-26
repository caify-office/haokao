using System.Collections.Generic;
using System.Collections.Immutable;
using k8s;
using Yarp.ReverseProxy.Configuration;

namespace HaoKao.YarpGateway.Services;

public class KubernetesClientService
{
    public static List<ClientServiceConfig> GetKubernetesClientServices()
    {
        // 创建 Kubernetes 客户端
        var kubernetesClientConfiguration = KubernetesClientConfiguration.InClusterConfig(); // 适用于 K8s 运行时
        var kubernetesClient = new Kubernetes(kubernetesClientConfiguration);

        var services = kubernetesClient.CoreV1.ListServiceForAllNamespacesAsync().Result;

        var result = new List<ClientServiceConfig>();
        foreach (var service in services.Items)
        {
            var serviceName = service.Metadata.Name;
            // 创建不可变字典
            var destinationBuilder = ImmutableDictionary.CreateBuilder<string, DestinationConfig>();

            foreach (var port in service.Spec.Ports)
            {
                destinationBuilder.Add(
                    $"{serviceName}-{port.Port}",
                    new DestinationConfig
                    {
                        Address =
                            $"http://{serviceName}.{service.Metadata.NamespaceProperty}.svc.cluster.local:{port.Port}"
                    }
                );
            }

            result.Add(
                new ClientServiceConfig
                {
                    ServiceName = serviceName,
                    Destinations = destinationBuilder.ToImmutable()
                }
            );
        }

        return result;
    }
}
