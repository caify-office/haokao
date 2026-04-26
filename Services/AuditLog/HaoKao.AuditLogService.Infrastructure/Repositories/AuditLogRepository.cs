using HaoKao.AuditLogService.Domain.Entities;
using HaoKao.AuditLogService.Domain.Repositories;

namespace HaoKao.AuditLogService.Infrastructure.Repositories;

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository()
    {

    }
}