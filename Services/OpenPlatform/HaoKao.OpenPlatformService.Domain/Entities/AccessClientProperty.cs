namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 属性
/// </summary>
public class AccessClientProperty : AggregateRoot<Guid>
{
    /// <summary>
    /// 键
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}