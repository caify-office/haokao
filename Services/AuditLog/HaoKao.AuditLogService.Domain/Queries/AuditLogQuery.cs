

using HaoKao.AuditLogService.Domain.Entities;
using HaoKao.AuditLogService.Domain.Enumerations;

namespace HaoKao.AuditLogService.Domain.Queries;

public class AuditLogQuery : QueryBase<AuditLog>
{
    [QueryCacheKey]
    public DateTime? StartDateTime { get; set; }

    [QueryCacheKey]
    public DateTime? EndDateTime { get; set; }

    [QueryCacheKey]
    public string ServiceModuleName { get; set; }

    [QueryCacheKey]
    public string Message { get; set; }

    [QueryCacheKey]
    public string MessageContent { get; set; }

    [QueryCacheKey]
    public string CreatorName { get; set; }
        
    [QueryCacheKey]
    public string AddressIp { get; set; }
        
    [QueryCacheKey]
    public SourceType? SourceType { get; set; }

    public override Expression<Func<AuditLog, bool>> GetQueryWhere()
    {
        Expression<Func<AuditLog, bool>> expression = x => true;

        if (SourceType.HasValue)
        {
            expression = expression.And(x => x.SourceType == SourceType);
        }
            
        if (StartDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartDateTime);
        }

        if (EndDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime <= ((DateTime)EndDateTime).AddHours(23).AddMinutes(59).AddSeconds(59));
        }

        if (!string.IsNullOrWhiteSpace(AddressIp))
        {
            expression = expression.And(x => x.AddressIp.Contains(AddressIp));
        }

        if (!string.IsNullOrWhiteSpace(CreatorName))
        {
            expression = expression.And(x => x.CreatorName.Contains(CreatorName));
        }

        if (!string.IsNullOrWhiteSpace(Message))
        {
            expression = expression.And(x => x.Message.Contains(Message));
        }
            
        if (!string.IsNullOrWhiteSpace(MessageContent))
        {
            expression = expression.And(x => x.MessageContent.Contains(MessageContent));
        }

        if (!string.IsNullOrWhiteSpace(ServiceModuleName))
        {
            expression = expression.And(x => x.ServiceModuleName.Contains(ServiceModuleName));
        }
        return expression;
    }
}