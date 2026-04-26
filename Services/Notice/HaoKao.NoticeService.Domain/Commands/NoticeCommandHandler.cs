using HaoKao.NoticeService.Domain.Models;
using HaoKao.NoticeService.Domain.Repositories;

namespace HaoKao.NoticeService.Domain.Commands;

public class NoticeCommandHandler(
    IUnitOfWork<Notice> uow,
    IMediatorHandler bus,
    INoticeRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateNoticeCommand, bool>,
    IRequestHandler<UpdateNoticeCommand, bool>,
    IRequestHandler<DeleteNoticeCommand, bool>,
    IRequestHandler<UpdateNoticePublishedCommand, bool>,
    IRequestHandler<UpdateNoticePopupCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateNoticeCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Notice>(request);

        await repository.AddAsync(entity);

        if (await Commit())
        {
            await RemoveNoticeCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateNoticeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应公告的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        mapper.Map(request, entity);

        if (await Commit())
        {
            await RemoveNoticeCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteNoticeCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteRangeAsync(x => request.Ids.Contains(x.Id));

        await RemoveNoticeCache(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(UpdateNoticePublishedCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.Published, request.Published));

        await RemoveNoticeCache(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(UpdateNoticePopupCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id),
                                            s => s.SetProperty(x => x.Popup, request.Popup)
                                                  .SetProperty(x => x.StartTime, request.StartTime)
                                                  .SetProperty(x => x.EndTime, request.EndTime));

        await RemoveNoticeCache(cancellationToken);

        return true;
    }

    private Task RemoveNoticeCache(CancellationToken cancellationToken)
    {
        var key = new CacheKey(nameof(Notice).ToLowerInvariant() + ":").Create();
        return _bus.RaiseEvent(new RemoveCacheListEvent(key), cancellationToken);
    }
}