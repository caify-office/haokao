using System.Collections.Generic;
using System.Linq;
using Yarp.ReverseProxy.Configuration;

namespace HaoKao.YarpGateway.Services;

public class ClientServiceConfig
{
    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServiceName { get; set; }

    public string Health { get; set; }

    public IReadOnlyDictionary<string, DestinationConfig>? Destinations { get; set; } =
        new Dictionary<string, DestinationConfig>();

    /// <summary>
    /// 获取完整的服务地址
    /// </summary>
    /// <returns></returns>
    public IList<string> GetCompleteServiceAddress()
    {
        return Destinations.Values.Select(x => x.Address).ToList();
    }
}
