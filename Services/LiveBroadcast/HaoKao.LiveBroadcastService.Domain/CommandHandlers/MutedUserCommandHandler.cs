using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.MutedUser;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class MutedUserCommandHandler(
    IUnitOfWork<MutedUser> uow,
    IMutedUserRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateMutedUserCommand, bool>,
    IRequestHandler<DeleteMutedUserCommand, bool>
{
    private readonly IMutedUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateMutedUserCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistEntityAsync(x => x.UserId == request.UserId))
        {
            await _bus.RaiseEvent(new DomainNotification(request.UserId.ToString(), "用户禁言中", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }

        var entity = _mapper.Map<MutedUser>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<MutedUser>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<MutedUser>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteMutedUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(w => w.UserId.Equals(request.UserId));

        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.UserId.ToString(), "未找到对应禁言用户的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(entity);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<MutedUser>.ByIdCacheKey.Create(entity.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<MutedUser>(cancellationToken);
        }

        return true;
    }
}