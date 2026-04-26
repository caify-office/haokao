namespace HaoKao.AuditLogService.Application.Services.Management;

public interface IAuditLogService:IAppWebApiService
{
    Task<DetailAuditLogViewModel> Get(Guid id);
    Task<AuditLogQueryViewModel> Get(AuditLogQueryViewModel queryViewModel);
}