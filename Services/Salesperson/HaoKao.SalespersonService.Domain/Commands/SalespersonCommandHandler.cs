using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Repositories;

namespace HaoKao.SalespersonService.Domain.Commands;

public class SalespersonCommandHander(
    IUnitOfWork<Salesperson> uow,
    IMediatorHandler bus,
    IMapper mapper,
    ISalespersonRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateSalespersonCommand, bool>,
    IRequestHandler<UpdateSalespersonCommand, bool>,
    IRequestHandler<DeleteSalespersonCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly ISalespersonRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateSalespersonCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Salesperson>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<Salesperson>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateSalespersonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应销售人员的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = _mapper.Map(request, entity);
        await _repository.UpdateAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<Salesperson>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteSalespersonCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIds(request.Ids);

        await Task.WhenAll(request.Ids.Select(x => _bus.RemoveEntityCache<Salesperson>(x, cancellationToken)));
        await _bus.RemoveListCache<Salesperson>(cancellationToken);

        return true;
    }
}