using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveOnlineUser;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveOnlineUserCommandHandler(
    IUnitOfWork<LiveOnlineUser> uow,
    ILiveOnlineUserRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveOnlineUserCommand, bool>,
    IRequestHandler<UpdateLiveOnlineUserCommand, bool>
{
    private readonly ILiveOnlineUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateLiveOnlineUserCommand request, CancellationToken cancellationToken)
    {
        // 不可重复添加用户
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        if (await _repository.ExistEntityAsync(x => x.CreatorId == userId && x.LiveId == request.LiveId))
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.LiveId.ToString(), "已存在对应直播在线用户的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        var entity = request.MapToEntity<LiveOnlineUser>();

        entity.IsOnline = true;
        entity.LastOnlineTime = DateTime.Now;

        // 解决 OnlineUserTrend 租户分表自动迁移的问题
        await EngineContext.Current.Resolve<ILiveOnlineUserTrendRepository>().GetAsync(x => true);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<LiveOnlineUser>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveOnlineUser>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLiveOnlineUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播在线用户的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (request.IsOnline && !entity.IsOnline)
        {
            // 更新最后上线时间
            entity.LastOnlineTime = DateTime.Now;
            entity.CreatorName = request.CreatorName;
        }
        else
        {
            // 累计在线时长
            entity.OnlineDuration += (int)(DateTime.Now - entity.LastOnlineTime).TotalSeconds;
        }

        entity.IsOnline = request.IsOnline;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<LiveOnlineUser>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveOnlineUser>(cancellationToken);
        }

        return true;
    }
}