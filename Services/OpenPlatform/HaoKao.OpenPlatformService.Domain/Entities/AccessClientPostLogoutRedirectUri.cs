namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 注销重定向 Uri 
/// </summary>
public class AccessClientPostLogoutRedirectUri : AggregateRoot<Guid>
{
    /// <summary>
    /// 注销重定向 Uri 
    /// </summary>
    public string PostLogoutRedirectUri { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}