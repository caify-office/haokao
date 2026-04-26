using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.CouponService.Domain.Commands.MarketingPersonnel;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.CouponService.Domain.CommandHandlers;

public class MarketingPersonnelCommandHandler(
    IUnitOfWork<MarketingPersonnel> uow,
    IMarketingPersonnelRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateMarketingPersonnelCommand, bool>,
    IRequestHandler<UpdateMarketingPersonnelCommand, bool>,
    IRequestHandler<DeleteMarketingPersonnelCommand, bool>
{
    private readonly IMarketingPersonnelRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateMarketingPersonnelCommand request, CancellationToken cancellationToken)
    {
        var marketingPersonnel = mapper.Map<MarketingPersonnel>(request);

        await _repository.AddAsync(marketingPersonnel);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<MarketingPersonnel>.ByIdCacheKey.Create(marketingPersonnel.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(marketingPersonnel, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<MarketingPersonnel>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateMarketingPersonnelCommand request, CancellationToken cancellationToken)
    {
        var marketingPersonnel = await _repository.GetByIdAsync(request.Id);
        if (marketingPersonnel == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        marketingPersonnel = mapper.Map(request, marketingPersonnel);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<MarketingPersonnel>.ByIdCacheKey.Create(marketingPersonnel.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(marketingPersonnel, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<MarketingPersonnel>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteMarketingPersonnelCommand request, CancellationToken cancellationToken)
    {
        var marketingPersonnel = await _repository.GetByIdAsync(request.Id);
        if (marketingPersonnel == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(marketingPersonnel);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<MarketingPersonnel>.ByIdCacheKey.Create(marketingPersonnel.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<MarketingPersonnel>(cancellationToken);
        }

        return true;
    }
}