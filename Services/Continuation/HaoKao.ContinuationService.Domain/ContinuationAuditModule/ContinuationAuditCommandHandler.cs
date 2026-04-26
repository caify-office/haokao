using HaoKao.Common.Events.StudentPermission;
using HaoKao.Common.Extensions;
using HaoKao.ContinuationService.Domain.ContinuationSetupModule;

namespace HaoKao.ContinuationService.Domain.ContinuationAuditModule;

public class ContinuationAuditCommandHandler(
    IUnitOfWork<ContinuationAudit> uow,
    IMediatorHandler bus,
    IContinuationAuditRepository repository,
    IContinuationSetupRepository continuationSetupRepository,
    IEventBus eventBus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateContinuationAuditCommand, bool>,
    IRequestHandler<UpdateContinuationAuditCommand, bool>
{
    private readonly IContinuationAuditRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IContinuationSetupRepository _continuationSetupRepository = continuationSetupRepository ?? throw new ArgumentNullException(nameof(continuationSetupRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// 创建续读审核
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(CreateContinuationAuditCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ContinuationAudit>(request);
        entity.AuditReason = string.Empty;

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 更新续读审核
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateContinuationAuditCommand request, CancellationToken cancellationToken)
    {
        var audit = await _repository.GetByIdAsync(request.Id);
        if (audit == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应续读审核的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        var setup = await _continuationSetupRepository.GetByIdAsync(audit.SetupId);
        if (setup == null)
        {
            var notification = new DomainNotification(audit.SetupId.ToString(), "未找到对应续读配置的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        if (audit.AuditState != AuditState.InAudit)
        {
            var notification = new DomainNotification(request.Id.ToString(), "已审核的数据", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        audit = _mapper.Map(request, audit);
        if (request.AuditState == AuditState.Pass)
        {
            audit.RestOfCount--;
        }

        if (await Commit())
        {
            await UpdateEntityCache(audit, cancellationToken);
            await RemoveListCache(cancellationToken);

            // 发送事件, 延长学员权限的过期期限
            if (request.AuditState == AuditState.Pass)
            {
                PublishUpdateStudentPermissionExpiryTimeEvent();
            }
        }

        return true;

        void PublishUpdateStudentPermissionExpiryTimeEvent()
        {
            // 更新学员产品权限的到期时间
            _eventBus.PublishAsync(new UpdateStudentPermissionExpiryTimeEvent(audit.CreatorId, audit.ProductId, setup.ExpiryTime));
            // 更新学员产品赠品权限的到期时间
            foreach (var gift in audit.ProductGifts)
            {
                _eventBus.PublishAsync(new UpdateStudentPermissionExpiryTimeEvent(audit.CreatorId, gift.To<Guid>(), setup.ExpiryTime));
            }
        }
    }

    #region 缓存操作

    private Task UpdateEntityCache(ContinuationAudit entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveListCacheEvent<ContinuationAudit>(cancellationToken);
    }

    #endregion
}