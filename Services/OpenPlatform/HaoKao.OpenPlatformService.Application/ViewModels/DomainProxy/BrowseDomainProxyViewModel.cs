namespace HaoKao.OpenPlatformService.Application.ViewModels.DomainProxy;

public class BrowseDomainProxyViewModel
{
    public Guid Id { get; set; }

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

    /// <summary>
    /// 客户端标识
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    public string ClientName { get; set; }

    /// <summary>
    /// 客户端描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 客户端Logo
    /// </summary>
    public string LogoUri { get; set; }
}