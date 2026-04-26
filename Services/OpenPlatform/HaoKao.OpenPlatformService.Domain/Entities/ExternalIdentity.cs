namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 其它平台唯一标识符
/// </summary>
public class ExternalIdentity : AggregateRoot<Guid>
{
    /// <summary>
    /// 其它平台名称
    /// </summary>
    public string Scheme { get; set; }

    /// <summary>
    /// 唯一标识符
    /// </summary>
    public string UniqueIdentifier { get; set; }

    /// <summary>
    /// 用户其它平台相关信息
    /// </summary>
    public Dictionary<string, string> OtherInformation { get; set; }

    /// <summary>
    /// 对应的用户
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public RegisterUser RegisterUser { get; set; }
}