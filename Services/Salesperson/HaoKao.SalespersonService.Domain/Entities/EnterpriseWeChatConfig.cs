namespace HaoKao.SalespersonService.Domain.Entities;

/// <summary>
/// 企业微信配置实体类
/// </summary>
public class EnterpriseWeChatConfig : AggregateRoot<Guid>,
                                      IIncludeMultiTenant<Guid>,
                                      IIncludeCreateTime,
                                      IIncludeUpdateTime
{
    /// <summary>
    /// 企业Id
    /// </summary>
    public string CorpId { get; set; }

    /// <summary>
    /// 企业名称
    /// </summary>
    public string CorpName { get; set; }

    /// <summary>
    /// 应用的凭证密钥
    /// </summary>
    public string CorpSecret { get; set; }

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