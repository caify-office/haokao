using HaoKao.Common.Extensions;
using HaoKao.FeedBackService.Domain.Commands.FeedBackReply;
using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Repositories;

namespace HaoKao.FeedBackService.Domain.CommandHandlers;

public class FeedBackReplyCommandHandler(
    IUnitOfWork<FeedBackReply> uow,
    IMediatorHandler bus,
    IFeedBackReplyRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateFeedBackReplyCommand, bool>,
    IRequestHandler<UpdateFeedBackReplyCommand, bool>,
    IRequestHandler<DeleteFeedBackReplyCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateFeedBackReplyCommand request, CancellationToken cancellationToken)
    {
        var entity = new FeedBackReply
        {
            ReplyContent = request.ReplyContent,
            ReplyUserName = request.ReplyUserName,
            FeedBackId = request.FeedBackId,
            FileUrl = request.FileUrl
        };
        await repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<FeedBackReply>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateFeedBackReplyCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应答疑回复的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        entity.ReplyContent = request.ReplyContent;
        entity.ReplyUserName = request.ReplyUserName;
        entity.FeedBackId = request.FeedBackId;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<FeedBackReply>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteFeedBackReplyCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应答疑回复的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(entity);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<FeedBackReply>(entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<FeedBackReply>(cancellationToken);
        }

        return true;
    }
}