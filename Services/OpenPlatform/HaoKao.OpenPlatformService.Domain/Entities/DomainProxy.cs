namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 域名代理设置
/// </summary>
public class DomainProxy : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, IIncludeMultiTenantName
{
    /// <summary>
    /// 域名
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    public Guid AccessClientId { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public AccessClient AccessClient { get; set; }
}