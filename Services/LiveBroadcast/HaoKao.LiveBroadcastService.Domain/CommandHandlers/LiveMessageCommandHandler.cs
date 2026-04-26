using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveMessageCommandHandler(
    IUnitOfWork<LiveMessage> uow,
    ILiveMessageRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveMessageCommand, bool>,
    IRequestHandler<DeleteLiveMessageCommand, bool>
{
    private readonly ILiveMessageRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLiveMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<LiveMessage>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveMessage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteLiveMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播消息的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(entity);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<LiveMessage>(entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveMessage>(cancellationToken);
        }

        return true;
    }
}