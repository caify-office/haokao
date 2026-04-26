using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.CouponService.Domain.Commands.Coupon;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.CouponService.Domain.CommandHandlers;

public class CouponCommandHandler(
    IUnitOfWork<Coupon> uow,
    ICouponRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCouponCommand, bool>,
    IRequestHandler<UpdateCouponCommand, bool>,
    IRequestHandler<DeleteCouponCommand, bool>
{
    private readonly ICouponRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        
        var coupon = mapper.Map<Coupon>(request);
        coupon.EndDate=coupon.EndDate.AddDays(1);
        coupon.CouponCode = DateTime.Now.Ticks.ToString();

        await _repository.AddAsync(coupon);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Coupon>.ByIdCacheKey.Create(coupon.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(coupon, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<Coupon>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _repository.GetByIdAsync(request.Id);
        if (coupon == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
  
        coupon = mapper.Map(request, coupon);
        coupon.EndDate = coupon.EndDate.AddDays(1);
        await _repository.UpdateAsync(coupon);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Coupon>.ByIdCacheKey.Create(coupon.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(coupon, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<Coupon>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _repository.GetByIdAsync(request.Id);
        if (coupon == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(coupon);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<Coupon>.ByIdCacheKey.Create(coupon.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<Coupon>(cancellationToken);
        }

        return true;
    }
}