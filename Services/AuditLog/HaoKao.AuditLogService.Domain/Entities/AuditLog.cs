using HaoKao.AuditLogService.Domain.Enumerations;

namespace HaoKao.AuditLogService.Domain.Entities;

public class AuditLog : AggregateRoot<Guid>,
                        IIncludeCreatorId<Guid>,
                        IIncludeCreatorName,
                        IIncludeMultiTenant<Guid>,
                        IIncludeCreateTime,
                        ITenantShardingTable,
                        IIncludeMultiTenantName
{
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
    /// Ip地址来源
    /// </summary>
    public string AddressIp { get; set; }

    /// <summary>
    /// 消息来源
    /// </summary>
    public SourceType SourceType { get; set; }

    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }

    public Guid TenantId { get; set; }

    public string TenantName { get; set; }

    public DateTime CreateTime { get; set; }
}