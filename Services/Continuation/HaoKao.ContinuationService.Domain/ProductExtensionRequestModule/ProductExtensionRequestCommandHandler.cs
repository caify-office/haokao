using Girvs.Infrastructure;
using HaoKao.Common.Events.StudentPermission;
using HaoKao.Common.Extensions;
using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

public class ProductExtensionRequestCommandHandler(
    IUnitOfWork<ProductExtensionRequest> uow,
    IMediatorHandler bus,
    IProductExtensionRequestRepository repository,
    IProductExtensionPolicyRepository policyRepository,
    IEventBus eventBus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateProductExtensionRequestCommand, bool>,
    IRequestHandler<UpdateProductExtensionRequestStateCommand, bool>
{
    private readonly IProductExtensionRequestRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IProductExtensionPolicyRepository _policyRepository = policyRepository ?? throw new ArgumentNullException(nameof(policyRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateProductExtensionRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductExtensionRequest>(request);

        // 初始化状态
        entity.AuditState = ProductExtensionRequestState.Waiting;
        entity.AuditReason = string.Empty;
        entity.AuditLogs = []; // 初始无日志

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateProductExtensionRequestStateCommand request, CancellationToken cancellationToken)
    {
        var extensionRequest = await _repository.GetByIdAsync(request.Id);
        if (extensionRequest == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应申请的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        var policy = await _policyRepository.GetByIdAsync(extensionRequest.PolicyId);
        if (policy == null)
        {
            var notification = new DomainNotification(extensionRequest.PolicyId.ToString(), "未找到对应策略的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        // 幂等性检查：如果已经是最终状态，不允许再修改
        if (extensionRequest.AuditState == ProductExtensionRequestState.Approved)
        {
            var notification = new DomainNotification(request.Id.ToString(), "该申请已审核通过，禁止重复审核", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var userName = EngineContext.Current.ClaimManager.GetUserName();

        // 1. 更新主表快照
        extensionRequest.AuditState = request.State;
        extensionRequest.AuditReason = request.Remark;
        extensionRequest.AuditTime = DateTime.Now;
        extensionRequest.AuditOperatorName = userName;

        // 2. 添加审核日志
        var log = new ProductExtensionAuditLog
        {
            RequestId = extensionRequest.Id,
            NewState = request.State,
            Remark = request.Remark,
            CreateTime = DateTime.Now,
            CreatorId = userId,
            CreatorName = userName,
        };

        // 确保 AuditLogs 集合已初始化
        extensionRequest.AuditLogs ??= [];
        extensionRequest.AuditLogs.Add(log);

        // 3. 业务逻辑处理 (如果是 Approved)
        if (request.State == ProductExtensionRequestState.Approved)
        {
            // 扣减次数
            extensionRequest.RestOfCount--;
        }

        if (await Commit())
        {
            await UpdateEntityCache(extensionRequest, cancellationToken);
            await RemoveListCache(cancellationToken);

            // 发送事件, 延长学员权限的过期期限
            if (request.State == ProductExtensionRequestState.Approved)
            {
                // 计算新的过期时间：固定日期直接使用 ExpiryDate，相对天数在原过期时间基础上累加
                var finalExpiryTime = policy.ExtensionType == ExtensionType.FixedDate
                    ? policy.ExpiryDate.Value
                    : extensionRequest.ExpiryTime.AddDays(policy.ExtensionDays.Value);

                PublishUpdateStudentPermissionExpiryTimeEvent(finalExpiryTime);
            }
        }

        return true;

        void PublishUpdateStudentPermissionExpiryTimeEvent(DateTime expiryTime)
        {
            // 更新学员产品权限的到期时间
            _eventBus.PublishAsync(new UpdateStudentPermissionExpiryTimeEvent(extensionRequest.CreatorId, extensionRequest.ProductId, expiryTime));

            // 更新学员产品赠品权限的到期时间
            foreach (var gift in extensionRequest.ProductGifts)
            {
                _eventBus.PublishAsync(new UpdateStudentPermissionExpiryTimeEvent(extensionRequest.CreatorId, gift.To<Guid>(), expiryTime));
            }
        }
    }

    #region 缓存操作

    private Task UpdateEntityCache(ProductExtensionRequest entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveListCacheEvent<ProductExtensionRequest>(cancellationToken);
    }

    #endregion
}