using HaoKao.AuditLogService.Domain.Entities;
using HaoKao.AuditLogService.Domain.Repositories;
using HaoKao.Common.Extensions;

namespace HaoKao.AuditLogService.Domain.Commands;

public class AuditLogCommandHandler(
    IUnitOfWork<AuditLog> uow,
    IMediatorHandler bus,
    IAuditLogRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateAuditLogCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IAuditLogRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
    {
        var log = new AuditLog
        {
            ServiceModuleName = request.ServiceModuleName,
            Message = request.Message,
            MessageContent = request.MessageContent,
            CreatorId = request.CreatorId,
            CreatorName = HttpUtility.UrlDecode(request.CreatorName),
            AddressIp = request.AddressIp,
            SourceType = request.SourceType
        };

        if (log.CreatorName.Length > 20)
        {
            log.CreatorName = string.Concat(log.CreatorName.AsSpan(0, 20), "...");
        }

        if (log.TenantName?.Length > 50)
        {
            log.TenantName = string.Concat(log.TenantName.AsSpan(0, 50), "...");
        }

        await _repository.AddAsync(log);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(log, log.Id.ToString(), cancellationToken);
        }

        return true;
    }
}