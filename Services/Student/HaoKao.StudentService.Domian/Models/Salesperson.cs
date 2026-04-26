namespace HaoKao.StudentService.Domain.Models;

/// <summary>
/// 销售人员
/// </summary>
public record Salesperson
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; init; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; init; }

    /// <summary>
    /// 企业微信用户Id
    /// </summary>
    public string EnterpriseWeChatUserId { get; init; }

    /// <summary>
    /// 企业微信昵称
    /// </summary>
    public string EnterpriseWeChatUserName { get; init; }

    /// <summary>
    /// 企业微信配置Id
    /// </summary>
    public Guid EnterpriseWeChatConfigId { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; init; }
}