using HaoKao.ProductService.Domain.Commands.SupervisorClass;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Domain.CommandHandlers;

public class SupervisorClassCommandHandler(
    IUnitOfWork<SupervisorClass> uow,
    ISupervisorClassRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateSupervisorClassCommand, bool>,
    IRequestHandler<UpdateSupervisorClassCommand, bool>,
    IRequestHandler<DeleteSupervisorClassCommand, bool>
{
    private readonly ISupervisorClassRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateSupervisorClassCommand request, CancellationToken cancellationToken)
    {
        var supervisorClass = _mapper.Map<SupervisorClass>(request);

        await _repository.AddAsync(supervisorClass);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<SupervisorClass>.ByIdCacheKey.Create(supervisorClass.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(supervisorClass, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<SupervisorClass>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateSupervisorClassCommand request, CancellationToken cancellationToken)
    {
        var supervisorClass = await _repository.GetByIdAsync(request.Id);
        if (supervisorClass == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应班级督学的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        supervisorClass = _mapper.Map(request, supervisorClass);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<SupervisorClass>.ByIdCacheKey.Create(supervisorClass.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(supervisorClass, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<SupervisorClass>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteSupervisorClassCommand request, CancellationToken cancellationToken)
    {
        var supervisorClass = await _repository.GetByIdAsync(request.Id);
        if (supervisorClass == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应班级督学的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(supervisorClass);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<SupervisorClass>.ByIdCacheKey.Create(supervisorClass.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<SupervisorClass>(cancellationToken);
        }

        return true;
    }
}