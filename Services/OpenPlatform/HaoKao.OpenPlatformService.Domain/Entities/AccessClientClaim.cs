namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 声明
/// </summary>
public class AccessClientClaim : AggregateRoot<Guid>
{
    /// <summary>
    /// 声明类型
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 声明值
    /// </summary>
    public string Value { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}