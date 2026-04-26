using HaoKao.Common.Extensions;
using HaoKao.FeedBackService.Domain.Commands.Suggestion;
using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Repositories;

namespace HaoKao.FeedBackService.Domain.CommandHandlers;

public class SuggestionCommandHandler(
    IUnitOfWork<Suggestion> uow,
    IMediatorHandler bus,
    ISuggestionRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateSuggestionCommand, bool>,
    IRequestHandler<ReplySuggestionCommand, bool>,
    IRequestHandler<CloseSuggestionCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateSuggestionCommand request, CancellationToken cancellationToken)
    {
        var entity = request.MapToEntity<Suggestion>();

        await repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Suggestion>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(ReplySuggestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应意见反馈的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        entity.ReplyContent = request.ReplyContent;
        entity.ReplyScreenshots = request.ReplyScreenshots;
        entity.ReplyUserId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        entity.ReplyUserName = EngineContext.Current.ClaimManager.GetUserName();
        entity.ReplyTime = DateTime.Now;
        entity.ReplyState = ReplyState.Replied;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Suggestion>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(CloseSuggestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应意见反馈的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        entity.ReplyState = ReplyState.Closed;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Suggestion>(cancellationToken);
        }

        return true;
    }
}