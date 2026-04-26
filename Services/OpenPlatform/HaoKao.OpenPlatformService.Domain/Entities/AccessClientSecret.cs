namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 客户端密钥
/// </summary>
public class AccessClientSecret : AggregateRoot<Guid>
{
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 密钥值 
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 到期
    /// </summary>
    public DateTime? Expiration { get; set; }

    /// <summary>
    /// 密钥类型
    /// </summary>
    public string Type { get; set; } = "SharedSecret";

    /// <summary>
    /// 哈希类型
    /// </summary>

    public string HashType { get; set; } = "Sha256";

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}