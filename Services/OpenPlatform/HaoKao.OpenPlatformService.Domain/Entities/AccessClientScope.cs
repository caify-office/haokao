namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 作用域
/// </summary>
public class AccessClientScope : AggregateRoot<Guid>
{
    /// <summary>
    ///  作用域
    /// </summary>
    public string Scope { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}