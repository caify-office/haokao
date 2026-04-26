using HaoKao.Common.Extensions;

namespace HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

public class ProductExtensionPolicyCommandHandler(
    IUnitOfWork<ProductExtensionPolicy> uow,
    IMediatorHandler bus,
    IProductExtensionPolicyRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateProductExtensionPolicyCommand, bool>,
    IRequestHandler<UpdateProductExtensionPolicyCommand, bool>,
    IRequestHandler<DeleteProductExtensionPolicyCommand, bool>,
    IRequestHandler<EnableProductExtensionPolicyCommand, bool>
{
    private readonly IProductExtensionPolicyRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateProductExtensionPolicyCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductExtensionPolicy>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateProductExtensionPolicyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应策略的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        // 业务规则：如果在有效期内，限制部分修改
        if (entity.StartTime <= DateTime.Today && entity.EndTime >= DateTime.Today)
        {
            // 如果策略已经开始，禁止把开始时间推迟（可以提前，但这里未做判断，简单处理为不能改）
            if (entity.StartTime != request.StartTime)
            {
                var notification = new DomainNotification(request.Id.ToString(), "策略已生效，禁止修改开始时间", StatusCodes.Status400BadRequest);
                await _bus.RaiseEvent(notification, cancellationToken);
                return false;
            }

            // 如果策略已开始，只能增加产品，不能删除已有产品（防止已申请的用户数据异常）
            var existingProductIds = entity.Products.Select(x => x.ProductId).ToList();
            var newProductIds = request.Products.Select(x => x.ProductId).ToList();
            if (!existingProductIds.All(id => newProductIds.Contains(id)))
            {
                var notification = new DomainNotification(request.Id.ToString(), "策略已生效，禁止移除已有产品", StatusCodes.Status400BadRequest);
                await _bus.RaiseEvent(notification, cancellationToken);
                return false;
            }
        }
        // 如果还没开始，但试图把开始时间设为过去
        else if (entity.StartTime > DateTime.Today && request.StartTime < DateTime.Today)
        {
            var notification = new DomainNotification(request.Id.ToString(), "策略开始时间不能早于今天", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = _mapper.Map(request, entity);
        // 显式赋值列表，确保更新
        entity.Products = request.Products;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteProductExtensionPolicyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应策略的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        if (entity.IsEnable)
        {
            var notification = new DomainNotification(request.Id.ToString(), "禁止删除启用状态的策略", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        // 策略生效期间禁止删除
        if (entity.StartTime <= DateTime.Now && entity.EndTime > DateTime.Now)
        {
            var notification = new DomainNotification(request.Id.ToString(), "禁止删除正在生效期间的策略", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity.IsDelete = true;

        await _repository.UpdateAsync(entity);

        if (await Commit())
        {
            await RemoveEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(EnableProductExtensionPolicyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应策略的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity.IsEnable = request.IsEnable;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    #region 缓存操作

    private Task UpdateEntityCache(ProductExtensionPolicy entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(ProductExtensionPolicy entity, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<ProductExtensionPolicy>(entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveListCacheEvent<ProductExtensionPolicy>(cancellationToken);
    }

    #endregion
}