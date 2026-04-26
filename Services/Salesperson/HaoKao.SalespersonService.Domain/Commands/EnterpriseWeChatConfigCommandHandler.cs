using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Repositories;

namespace HaoKao.SalespersonService.Domain.Commands;

public class EnterpriseWeChatConfigCommand(
    IUnitOfWork<EnterpriseWeChatConfig> uow,
    IMediatorHandler bus,
    IMapper mapper,
    IEnterpriseWeChatConfigRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateEnterpriseWeChatConfigCommand, bool>,
    IRequestHandler<UpdateEnterpriseWeChatConfigCommand, bool>,
    IRequestHandler<DeleteEnterpriseWeChatConfigCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IEnterpriseWeChatConfigRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateEnterpriseWeChatConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<EnterpriseWeChatConfig>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<EnterpriseWeChatConfig>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateEnterpriseWeChatConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应企业微信配置的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = _mapper.Map(request, entity);
        await _repository.UpdateAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<EnterpriseWeChatConfig>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteEnterpriseWeChatConfigCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIds(request.Ids);

        await Task.WhenAll(request.Ids.Select(x => _bus.RemoveEntityCache<EnterpriseWeChatConfig>(x, cancellationToken)));
        await _bus.RemoveListCache<EnterpriseWeChatConfig>(cancellationToken);

        return true;
    }
}