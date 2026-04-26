using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Girvs.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerUI;
using Yarp.ReverseProxy.Configuration;

namespace HaoKao.YarpGateway;

public class SwaggerEndpointEnumerator : IEnumerable<UrlDescriptor>
{
    public IEnumerator<UrlDescriptor> GetEnumerator()
    {
        var configProvider = EngineContext.Current.Resolve<IProxyConfigProvider>();
        var routes = configProvider.GetConfig().Routes;
        var swaggerConfig = routes
            .Select(router => new UrlDescriptor
            {
                Url = $"/{router.RouteId}/swagger/v1/swagger.json",
                Name = $"Service - {router.RouteId}"
            })
            .ToList();

        swaggerConfig.Insert(
            0,
            new UrlDescriptor { Url = "/swagger/v1/swagger.json", Name = "Gateway" }
        );

        foreach (var descriptor in swaggerConfig)
        {
            yield return descriptor;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
