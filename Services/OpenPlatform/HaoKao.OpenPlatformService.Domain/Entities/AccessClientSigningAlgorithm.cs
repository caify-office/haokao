namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 允许的授权类型
/// </summary>
public class AccessClientSigningAlgorithm : AggregateRoot<Guid>
{
    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }

    /// <summary>
    /// 授权类型
    /// </summary>
    public string SigningAlgorithm { get; set; }
}