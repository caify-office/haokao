namespace HaoKao.SalespersonService.Domain.Entities;

/// <summary>
/// 销售人员实体类
/// </summary>
public class Salesperson : AggregateRoot<Guid>,
                           IIncludeMultiTenant<Guid>,
                           IIncludeCreateTime,
                           IIncludeUpdateTime
{
    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 企业微信用户Id
    /// </summary>
    public string EnterpriseWeChatUserId { get; set; }

    /// <summary>
    /// 企业微信昵称
    /// </summary>
    public string EnterpriseWeChatUserName { get; set; }

    /// <summary>
    /// 企业微信配置Id
    /// </summary>
    public Guid EnterpriseWeChatConfigId { get; set; }

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

    /// <summary>
    /// 企业微信配置
    /// </summary>
    public EnterpriseWeChatConfig EnterpriseWeChatConfig { get; init; }
}