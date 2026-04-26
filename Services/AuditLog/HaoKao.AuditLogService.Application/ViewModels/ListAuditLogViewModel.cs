using HaoKao.AuditLogService.Domain.Entities;
using HaoKao.AuditLogService.Domain.Enumerations;

namespace HaoKao.AuditLogService.Application.ViewModels;

/// <summary>
/// 列表审计日志
/// </summary>
[AutoMapFrom(typeof(AuditLog))]
public class ListAuditLogViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 服务模块名称
    /// </summary>
    public string ServiceModuleName { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 具体消息内容
    /// </summary>
    [QueryIgnore]
    [Ignore]
    public string MessageContent { get; set; }

    /// <summary>
    /// 操作者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 操作者所属租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string AddressIp { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public SourceType? SourceType { get; set; }
}