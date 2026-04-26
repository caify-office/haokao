using HaoKao.AuditLogService.Domain.Enumerations;

namespace HaoKao.AuditLogService.Domain.Commands;

/// <summary>
/// 创建审计日志
/// </summary>
/// <param name="ServiceModuleName">服务模块名称</param>
/// <param name="Message">消息</param>
/// <param name="MessageContent">具体消息内容</param>
/// <param name="CreatorId">创建者Id</param>
/// <param name="CreatorName">创建者名称</param>
/// <param name="AddressIp">Ip地址来源</param>
/// <param name="SourceType">消息来源</param>
public record CreateAuditLogCommand(
    string ServiceModuleName,
    string Message,
    string MessageContent,
    Guid CreatorId,
    string CreatorName,
    string AddressIp,
    SourceType SourceType
) : Command(string.Empty);