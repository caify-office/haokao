using HaoKao.AuditLogService.Domain.Entities;
using HaoKao.AuditLogService.Domain.Enumerations;

namespace HaoKao.AuditLogService.Application.ViewModels;

[AutoMapFrom(typeof(AuditLog))]
public class DetailAuditLogViewModel : IDto
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
    public string MessageContent { get; set; }

    /// <summary>
    /// 操作者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 操作者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 操作者所属租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 操作者所属租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 操作者IP地址
    /// </summary>
    public string AddressIp { get; set; }

    /// <summary>
    /// 消息来源
    /// </summary>
    public SourceType SourceType { get; set; }
}