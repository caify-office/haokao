namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 允许跨域来源
/// </summary>
public class AccessClientCorsOrigin : AggregateRoot<Guid>
{
    /// <summary>
    /// 来源
    /// </summary>
    public string Origin { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}