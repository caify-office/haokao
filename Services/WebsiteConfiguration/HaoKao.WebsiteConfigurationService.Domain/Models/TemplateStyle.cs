namespace HaoKao.WebsiteConfigurationService.Domain.Models;

/// <summary>
/// 模板样式
/// </summary>
public class TemplateStyle : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 域名
    /// </summary>
    public string DomainName { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }
   
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}
