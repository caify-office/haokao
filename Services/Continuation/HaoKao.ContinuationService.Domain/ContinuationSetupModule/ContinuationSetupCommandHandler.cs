namespace HaoKao.ContinuationService.Domain.ContinuationSetupModule;

public class ContinuationSetupCommandHandler(
    IUnitOfWork<ContinuationSetup> uow,
    IMediatorHandler bus,
    IContinuationSetupRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateContinuationSetupCommand, bool>,
    IRequestHandler<UpdateContinuationSetupCommand, bool>,
    IRequestHandler<DeleteContinuationSetupCommand, bool>,
    IRequestHandler<EnableContinuationSetupCommand, bool>,
    IRequestHandler<UpdateExpiryTimeCommand, bool>
{
    private readonly IContinuationSetupRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// 创建续读配置
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(CreateContinuationSetupCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ContinuationSetup>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 更新续读配置
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateContinuationSetupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应续读配置的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        if (entity.StartTime <= DateTime.Today)
        {
            // 到开始时间后不能再次修改开始时间
            if (entity.StartTime != request.StartTime)
            {
                var notification = new DomainNotification(request.Id.ToString(), "申请开始后禁止修改开始时间", StatusCodes.Status400BadRequest);
                await _bus.RaiseEvent(notification, cancellationToken);
                return false;
            }

            // 到开始时间后只能添加产品不能删除
            if (!entity.Products.All(x => request.Products.Select(p => p.ProductId).Contains(x.ProductId)))
            {
                var notification = new DomainNotification(request.Id.ToString(), "申请开始后禁止删除产品", StatusCodes.Status400BadRequest);
                await _bus.RaiseEvent(notification, cancellationToken);
                return false;
            }
        }
        else if (request.StartTime < DateTime.Today)
        {
            var notification = new DomainNotification(request.Id.ToString(), "续读申请开始时间不能早于当前系统时间", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = _mapper.Map(request, entity);
        entity.Products = request.Products;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 删除续读配置
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(DeleteContinuationSetupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应续读配置的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        if (entity.Enable)
        {
            var notification = new DomainNotification(request.Id.ToString(), "禁止删除启用状态的续读配置数据", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        // 申请开始后到结束前禁止删除
        if (entity.StartTime <= DateTime.Now && entity.EndTime > DateTime.Now)
        {
            var notification = new DomainNotification(request.Id.ToString(), "禁止删除未到申请结束时间的续读配置数据", StatusCodes.Status400BadRequest);
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

    /// <summary>
    /// 启用/禁用续读配置
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(EnableContinuationSetupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应续读配置的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity.Enable = request.Enable;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 设置续读后会员到期时间
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateExpiryTimeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应续读配置的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity.ExpiryTime = request.ExpiryTime;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    #region 缓存操作

    private Task UpdateEntityCache(ContinuationSetup entity, CancellationToken cancellationToken)
    {
        var key = GirvsEntityCacheDefaults<ContinuationSetup>.ByIdCacheKey.Create(entity.Id.ToString());
        return _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
    }

    private Task RemoveEntityCache(ContinuationSetup entity, CancellationToken cancellationToken)
    {
        var key = GirvsEntityCacheDefaults<ContinuationSetup>.ByIdCacheKey.Create(entity.Id.ToString());
        return _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        var listKey = GirvsEntityCacheDefaults<ContinuationSetup>.ListCacheKey.Create();
        return _bus.RaiseEvent(new RemoveCacheListEvent(listKey), cancellationToken);
    }

    #endregion
}