using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.CacheKeyManager;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveAdministrator;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveAdministratorCommandHandler(
    IUnitOfWork<LiveAdministrator> uow,
    ILiveAdministratorRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveAdministratorCommand, bool>,
    IRequestHandler<DeleteLiveAdministratorCommand, bool>
{
    private readonly ILiveAdministratorRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLiveAdministratorCommand request, CancellationToken cancellationToken)
    {
        var liveAdministrator = _mapper.Map<LiveAdministrator>(request);

        await _repository.AddAsync(liveAdministrator);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<LiveAdministrator>.ByIdCacheKey.Create(liveAdministrator.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(liveAdministrator, key, key.CacheTime), cancellationToken);

            // 创建缓存Key
            var isAdminCacheKey = LiveAdministratorCacheKeyManager.CreateAdminCacheKey(liveAdministrator.UserId);
            // 将是否是管理员放入缓存中
            await _bus.RaiseEvent(new SetCacheEvent(true, isAdminCacheKey, key.CacheTime), cancellationToken);

            await _bus.RemoveListCacheEvent<LiveAdministrator>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteLiveAdministratorCommand request, CancellationToken cancellationToken)
    {
        var liveAdministrator = await _repository.GetByIdAsync(request.Id);
        if (liveAdministrator == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播管理员的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(liveAdministrator);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<LiveAdministrator>.ByIdCacheKey.Create(liveAdministrator.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);

            var isAdminCacheKey = LiveAdministratorCacheKeyManager.CreateAdminCacheKey(liveAdministrator.UserId);
            await _bus.RaiseEvent(new RemoveCacheEvent(isAdminCacheKey), cancellationToken);

            await _bus.RemoveListCacheEvent<LiveAdministrator>(cancellationToken);
        }

        return true;
    }
}