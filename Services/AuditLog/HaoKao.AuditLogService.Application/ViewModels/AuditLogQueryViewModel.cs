using HaoKao.AuditLogService.Domain.Enumerations;
using HaoKao.AuditLogService.Domain.Queries;

namespace HaoKao.AuditLogService.Application.ViewModels;

[AutoMapFrom(typeof(AuditLogQuery))]
[AutoMapTo(typeof(AuditLogQuery))]
public class AuditLogQueryViewModel : QueryDtoBase<ListAuditLogViewModel>
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndDateTime { get; set; }

    /// <summary>
    /// 服务模块名称
    /// </summary>
    public string ServiceModuleName { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 具体消息内容
    /// </summary>
    public string MessageContent { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string AddressIp { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public SourceType? SourceType { get; set; }
}