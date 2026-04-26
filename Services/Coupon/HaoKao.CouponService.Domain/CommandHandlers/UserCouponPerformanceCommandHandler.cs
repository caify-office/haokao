using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.CouponService.Domain.Commands.UserCouponPerformance;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.CouponService.Domain.CommandHandlers;

public class UserCouponPerformanceCommandHandler(
    IUnitOfWork<UserCouponPerformance> uow,
    IUserCouponPerformanceRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateUserCouponPerformanceCommand, bool>,
    IRequestHandler<UpdateUserCouponPerformanceCommand, bool>,
    IRequestHandler<DeleteUserCouponPerformanceCommand, bool>
{
    private readonly IUserCouponPerformanceRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateUserCouponPerformanceCommand request, CancellationToken cancellationToken)
    {
        //根据订单+产品名称验证是否重复
        var result = _repository.GetWhereAsync(x => x.OrderId == request.OrderId && x.ProductName == request.ProductName);
        if (result.Result.Count <= 0)
        {
            //同一笔订单下面  不可以重复添加相同的产品
            var userCouponPerformance = mapper.Map<UserCouponPerformance>(request);
            if (request.Id != Guid.Empty)
            {
                userCouponPerformance.Id = request.Id;
                userCouponPerformance.Remark = "实名优惠券";
            }
            else
            {
                userCouponPerformance.Remark = "后台添加";
            }
            await _repository.AddAsync(userCouponPerformance);
            if (await Commit())
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<UserCouponPerformance>.ByIdCacheKey.Create(userCouponPerformance.Id.ToString());
                // 将新增的纪录放到缓存中
                await _bus.RaiseEvent(new SetCacheEvent(userCouponPerformance, key, key.CacheTime), cancellationToken);
                   await _bus.RemoveListCacheEvent<UserCouponPerformance>(cancellationToken);
            }
        }
        return true;
    }

    public async Task<bool> Handle(UpdateUserCouponPerformanceCommand request, CancellationToken cancellationToken)
    {
        var userCouponPerformance = await _repository.GetByIdAsync(request.Id);
        if (userCouponPerformance == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        userCouponPerformance = mapper.Map(request, userCouponPerformance);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<UserCouponPerformance>.ByIdCacheKey.Create(userCouponPerformance.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(userCouponPerformance, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<UserCouponPerformance>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteUserCouponPerformanceCommand request, CancellationToken cancellationToken)
    {
        var userCouponPerformance = await _repository.GetByIdAsync(request.Id);
        if (userCouponPerformance == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(userCouponPerformance);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<UserCouponPerformance>.ByIdCacheKey.Create(userCouponPerformance.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<UserCouponPerformance>(cancellationToken);
        }

        return true;
    }
}