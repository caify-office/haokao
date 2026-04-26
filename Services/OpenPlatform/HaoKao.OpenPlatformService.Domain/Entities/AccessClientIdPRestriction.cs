namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 身份提供程序限制
/// </summary>
public class AccessClientIdPRestriction : AggregateRoot<Guid>
{
    /// <summary>
    /// 提供者
    /// </summary>
    public string Provider { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}