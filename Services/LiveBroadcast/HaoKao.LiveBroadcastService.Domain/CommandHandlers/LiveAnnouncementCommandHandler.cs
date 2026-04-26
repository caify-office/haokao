using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveAnnouncement;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveAnnouncementCommandHandler(
    IUnitOfWork<LiveAnnouncement> uow,
    ILiveAnnouncementRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveAnnouncementCommand, bool>,
    IRequestHandler<UpdateLiveAnnouncementCommand, bool>,
    IRequestHandler<DeleteLiveAnnouncementCommand, bool>
{
    private readonly ILiveAnnouncementRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLiveAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var liveAnnouncement = _mapper.Map<LiveAnnouncement>(request);

        await _repository.AddAsync(liveAnnouncement);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(liveAnnouncement, liveAnnouncement.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveAnnouncement>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLiveAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var liveAnnouncement = await _repository.GetByIdAsync(request.Id);
        if (liveAnnouncement == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播公告的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        liveAnnouncement = _mapper.Map(request, liveAnnouncement);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(liveAnnouncement, liveAnnouncement.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveAnnouncement>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteLiveAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var liveAnnouncement = await _repository.GetByIdAsync(request.Id);
        if (liveAnnouncement == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播公告的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(liveAnnouncement);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<LiveAnnouncement>(liveAnnouncement.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveAnnouncement>(cancellationToken);
        }

        return true;
    }
}