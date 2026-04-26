using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveComment;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveCommentCommandHandler(
    IUnitOfWork<LiveComment> uow,
    ILiveCommentRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveCommentCommand, bool>
{
    private readonly ILiveCommentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateLiveCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();

        if (await _repository.ExistEntityAsync(x => x.LiveId == request.LiveId && x.CreatorId == userId))
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.LiveId.ToString(), "已经评论过该直播", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        var entity = request.MapToEntity<LiveComment>();

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<LiveComment>(entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveComment>(cancellationToken);
        }

        return true;
    }
}