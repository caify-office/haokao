using AutoMapper;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.CacheKeyManager;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveReservation;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveReservationCommandHandler(
    IUnitOfWork<LiveReservation> uow,
    ILiveReservationRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveReservationCommand, bool>
{
    private readonly ILiveReservationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLiveReservationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<LiveReservation>(request);

        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        if (await _repository.ExistEntityAsync(x => x.CreatorId == userId && x.ProductId == request.ProductId))
        {
            await _bus.RaiseEvent(new DomainNotification(request.ProductId.ToString(), "已预约直播", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<LiveReservation>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);

            // 删除缓存Key
            key = LiveReservationCacheKeyManager.CreateMyLiveReservationCacheKey(entity.CreatorId);
            // 删除我的预约缓存
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);

           await _bus.RemoveListCacheEvent<LiveReservation>(cancellationToken);
        }

        return true;
    }
}