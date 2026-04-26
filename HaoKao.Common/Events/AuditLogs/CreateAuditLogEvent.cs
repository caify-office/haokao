namespace HaoKao.Common.Events.AuditLogs;

public record CreateAuditLogEvent(
    string ServiceModuleName,
    string Message,
    SourceType SourceType,
    string AddressIp,
    Guid TenantId,
    string TenantName,
    string MessageContent,
    Guid CreatorId,
    string CreatorName
) : IntegrationEvent;

public enum SourceType
{
    /// <summary>
    /// 服务端
    /// </summary>
    Server,

    /// <summary>
    /// 考生端
    /// </summary>
    Register,
}