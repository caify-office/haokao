using HaoKao.Common.Extensions;
using HaoKao.FeedBackService.Domain.Commands.FeedBack;
using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Repositories;

namespace HaoKao.FeedBackService.Domain.CommandHandlers;

public class FeedBackCommandHandler(
    IUnitOfWork<FeedBack> uow,
    IMediatorHandler bus,
    IFeedBackRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateFeedBackCommand, bool>,
    IRequestHandler<UpdateFeedBackCommand, bool>,
    IRequestHandler<DeleteFeedBackCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateFeedBackCommand request, CancellationToken cancellationToken)
    {
        var feedBack = new FeedBack
        {
            Type = request.Type,
            SourceType = request.SourceType,
            Status = request.Status,
            Contract = request.Contract,
            Description = request.Description,
            FileUrls = request.FileUrls,
            ParentId = request.ParentId
        };

        await repository.AddAsync(feedBack);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(feedBack, feedBack.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<FeedBack>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateFeedBackCommand request, CancellationToken cancellationToken)
    {
        var feedBack = await repository.GetByIdAsync(request.Id);
        if (feedBack == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        feedBack.Status = request.Status;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(feedBack, feedBack.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<FeedBack>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteFeedBackCommand request, CancellationToken cancellationToken)
    {
        var feedBack = await repository.GetByIdAsync(request.Id);
        if (feedBack == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(feedBack);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<FeedBack>(feedBack.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<FeedBack>(cancellationToken);
        }

        return true;
    }
}