using HaoKao.Common.Extensions;
using HaoKao.OrderService.Domain.Commands.PlatformPayer;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Repositories;

namespace HaoKao.OrderService.Domain.CommandHandlers;

public class PlatformPayerCommandHandler(
    IUnitOfWork<PlatformPayer> uow,
    IPlatformPayerRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreatePlatformPayerCommand, bool>,
    IRequestHandler<UpdatePlatformPayerCommand, bool>,
    IRequestHandler<SetPlatformPayerUseStateCommand, bool>,
    IRequestHandler<DeletePlatformPayerCommand, bool>
{
    private readonly IPlatformPayerRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreatePlatformPayerCommand request, CancellationToken cancellationToken)
    {
        var platformPayer = new PlatformPayer
        {
            Name = request.Name,
            PayerId = request.PayerId,
            PayerName = request.PayerName,
            PlatformPayerScenes = request.PlatformPayerScenes,
            PaymentMethod = request.PaymentMethod,
            IosIsOpen = request.IosIsOpen,
            Config = request.Config
        };

        platformPayer.SecurityCode = platformPayer.BuildSecurityCode();

        await _repository.AddAsync(platformPayer);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<PlatformPayer>.ByIdCacheKey.Create(platformPayer.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(platformPayer, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<PlatformPayer>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdatePlatformPayerCommand request, CancellationToken cancellationToken)
    {
        var platformPayer = await _repository.GetByIdAsync(request.Id);
        if (platformPayer == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应平台配置的支付列表的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        platformPayer.Name = request.Name;
        platformPayer.PayerId = request.PayerId;
        platformPayer.PayerName = request.PayerName;
        platformPayer.PaymentMethod = request.PaymentMethod;
        platformPayer.IosIsOpen = request.IosIsOpen;
        platformPayer.PlatformPayerScenes = request.PlatformPayerScenes;
        platformPayer.Config = request.Config;
        platformPayer.SecurityCode = platformPayer.BuildSecurityCode();
        platformPayer.UpdateTime = DateTime.Now;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<PlatformPayer>.ByIdCacheKey.Create(platformPayer.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(platformPayer, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<PlatformPayer>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetPlatformPayerUseStateCommand request, CancellationToken cancellationToken)
    {
        var platformPayer = await _repository.GetByIdAsync(request.Id);
        if (platformPayer == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应平台配置的支付列表的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        platformPayer.UseState = request.UseState;
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<PlatformPayer>.ByIdCacheKey.Create(platformPayer.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(platformPayer, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<PlatformPayer>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeletePlatformPayerCommand request, CancellationToken cancellationToken)
    {
        var platformPayer = await _repository.GetByIdAsync(request.Id);
        if (platformPayer == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应平台配置的支付列表的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(platformPayer);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<PlatformPayer>.ByIdCacheKey.Create(platformPayer.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<PlatformPayer>(cancellationToken);
        }

        return true;
    }
}