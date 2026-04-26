namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 重定向 Uri
/// </summary>
public class AccessClientRedirectUri : AggregateRoot<Guid>
{
    /// <summary>
    /// 重定向 Uri
    /// </summary>
    public string RedirectUri { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}