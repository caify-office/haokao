using AutoMapper;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.CouponService.Domain.Commands.UserCoupon;
using HaoKao.CouponService.Domain.Enumerations;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Queries;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.CouponService.Domain.CommandHandlers;

public class UserCouponCommandHandler(
    ILogger<UserCouponCommandHandler> logger,
    IUnitOfWork<UserCoupon> uow,
    IUserCouponRepository repository,
    ICouponRepository couponRepository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateUserCouponCommand, bool>,
    IRequestHandler<CreateUserCouponEventCommand, bool>,
    IRequestHandler<UpdateUserCouponCommand, bool>,
    IRequestHandler<DeleteUserCouponCommand, bool>,
    IRequestHandler<UpdateIsLockedNewCommand, bool>,
    IRequestHandler<UpdateCancelLockedCommand, bool>,
    IRequestHandler<UpdateIsUseCommand, bool>
{
    private readonly ICouponRepository _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
    private readonly IUserCouponRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<UserCouponCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<bool> Handle(CreateUserCouponCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var couponIds = request.CouponId.Split(',').Select(Guid.Parse).ToList();

        //去重
        couponIds = couponIds.Distinct().ToList();
        var couponList = await _couponRepository.GetWhereAsync(x => couponIds.Contains(x.Id));

        //当前用户领取相关的未使用且未过期的优惠券
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var noUseNoExpiredUserCoupon = await _repository.GetWhereAsync(x => x.CreatorId == userId && x.IsUse == false && couponIds.Contains(x.CouponId) && x.EndTime > now);

        //整理需要添加的用户优惠价
        var list = new List<UserCoupon>();
        foreach (var couponId in couponIds)
        {
            //验证优惠券是否存在
            var coupon = couponList.Find(x => x.Id == couponId);
            if (coupon == null)
            {
                continue;
            }

            //如果领取过期了的 依旧可以领取,如存在未过期并且未使用  那么就不可以领取
            if (!noUseNoExpiredUserCoupon.Any(x => x.CouponId == couponId))
            {
                var endTime = coupon.TimeType == TimeTypeEnum.Date ? coupon.EndDate : now.AddHours(coupon.Hour);
                var beginTime = coupon.TimeType == TimeTypeEnum.Date ? coupon.BeginDate : now;
                var userCoupon = new UserCoupon
                {
                    Remark = request.Remark,
                    CouponId = couponId,
                    IsUse = false,
                    NickName = request.NickName,
                    FactAmount = request.FactAmount,
                    ProductName = request.ProductName,
                    TelPhone = request.TelPhone,
                    IsLock = false,
                    EndTime = endTime,
                    BeginTime = beginTime,
                    CreateTime = now,
                    ChannelType = request.ChannelType,
                    Notified = false,
                    OpenId = request.OpenId,
                };

                list.Add(userCoupon);
            }
        }

        await _repository.AddRangeAsync(list);
        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<UserCoupon>(cancellationToken);

           await _bus.RemoveListCacheEvent<Coupon>(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<bool> Handle(CreateUserCouponEventCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;

        //当前用户领取相关的未使用且未过期的优惠券
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var noUseNoExpiredUserCoupon = await _repository.GetWhereAsync(x => x.CreatorId == userId
                                                                         && x.IsUse == false
                                                                         && x.CouponId == request.CouponId
                                                                         && x.EndTime > now);

        var couponId = request.CouponId;
        var coupon = await _couponRepository.GetByIdAsync(couponId);

        //如果领取过期了的 依旧可以领取,如存在未过期并且未使用  那么就不可以领取
        if (!noUseNoExpiredUserCoupon.Any(x => x.CouponId == couponId))
        {
            var endTime = coupon.TimeType == TimeTypeEnum.Date ? coupon.EndDate : now.AddHours(coupon.Hour);
            var beginTime = coupon.TimeType == TimeTypeEnum.Date ? coupon.BeginDate : now;
            var userCoupon = new UserCoupon
            {
                CouponId = couponId,
                IsUse = false,
                NickName = request.NickName,
                IsLock = false,
                EndTime = endTime,
                BeginTime = beginTime,
                CreateTime = now,
                ChannelType = request.ChannelType,
                Notified = false,
            };
            await _repository.AddAsync(userCoupon);
        }

        if (await Commit())
        {
            await _bus.RemoveListCacheEvent<UserCoupon>(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<bool> Handle(UpdateUserCouponCommand request, CancellationToken cancellationToken)
    {
        var userCoupon = await _repository.GetByIdAsync(request.Id);
        if (userCoupon == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
        userCoupon = mapper.Map(request, userCoupon);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<UserCoupon>.ByIdCacheKey.Create(userCoupon.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(userCoupon, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<UserCoupon>(cancellationToken);

           await _bus.RemoveListCacheEvent<Coupon>(cancellationToken);
        }
        return true;
    }

    /// <summary>
    /// 订单过期/取消订单根据订单id更新锁定状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateCancelLockedCommand request, CancellationToken cancellationToken)
    {
        var usercoupons = await _repository.GetUserCouponByOrderId(request.OrderId);
        usercoupons.ForEach(usercoupon => { usercoupon.IsLock = request.IsLocked; });

        if (await Commit())
        {
            usercoupons.ForEach(usercoupon =>
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<UserCoupon>.ByIdCacheKey.Create(usercoupon.Id.ToString());
                // 将新增的纪录放到缓存中
                _bus.RaiseEvent(new SetCacheEvent(usercoupon, key, key.CacheTime), cancellationToken);
            });
           await _bus.RemoveListCacheEvent<UserCoupon>(cancellationToken);
        }
        return true;
    }

    /// <summary>
    /// 添加订单更新这边的订单id和锁定状态根据用户id和优惠券ids集合
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateIsLockedNewCommand request, CancellationToken cancellationToken)
    {
        var userid = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        //需要过滤已使用,不然会重复更新,还要过滤未过期的
        var usercoupons = await _repository.GetUserCouponListForOrderLock(request.CouponIds);
        _logger.LogInformation("需要更新的count:" + usercoupons.Count);

        usercoupons.ForEach(x =>
        {
            x.OrderId = request.OrderId;
            x.IsLock = true;
        });

        if (await Commit())
        {
            usercoupons.ForEach(usercoupon =>
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<UserCoupon>.ByIdCacheKey.Create(usercoupon.Id.ToString());
                // 将新增的纪录放到缓存中
                _bus.RaiseEvent(new SetCacheEvent(usercoupon, key, key.CacheTime), cancellationToken);
            });

           await _bus.RemoveListCacheEvent<UserCoupon>(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 支付成功回调根据订单id更新使用,支付时间,金额等信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateIsUseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("进入到执行UpdateIsUseCommand命令方法");
        _logger.LogInformation("传过来的订单id:" + request.Id);
        //反序列订单产品集合 根据couponid拿到对应的金额 产品id  以及产品名称
        var proudctContents = JsonConvert.DeserializeObject<List<PurchaseProductContent>>(request.ProductContent);
        List<UserCoupon> usercoupons = null;
        if (request.FactAmount == 0)
        {
            var couponIds = new List<Guid>();
            proudctContents.ForEach(x =>
            {
                if (x.Coupons != null) couponIds.AddRange(x.Coupons.Select(p => p.Id));
            });
            //订单金额为0的情况
            usercoupons = await _repository.GetUserCouponListForOrderLock(string.Join(",", couponIds));
        }
        else
        {
            usercoupons = await _repository.GetUserCouponByOrderId(request.Id);
        }

        usercoupons.ForEach(usercoupon =>
        {
            var productModel = proudctContents.Where(x => x.Coupons != null && x.Coupons.Select(p => p.Id).ToList().Contains(usercoupon.CouponId)).FirstOrDefault();
            usercoupon.ProductId = productModel.ContentId; //产品包id->具体的产品id
            usercoupon.OrderNo = request.OrderNumber;
            usercoupon.IsUse = true;
            usercoupon.PayTime = request.PayTime;
            usercoupon.ProductName = productModel.ContentName; //产品包名称->产品名称
            usercoupon.Amount = productModel.ContentAmount; //订单金额->具体的产品金额
            usercoupon.FactAmount = productModel.ActualAmount; //订单金额->具体的产品金额
            usercoupon.OrderId = request.Id;
            usercoupon.IsLock = true;
        });

        if (await Commit())
        {
            usercoupons.ForEach(usercoupon =>
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<UserCoupon>.ByIdCacheKey.Create(usercoupon.Id.ToString());
                // 将新增的纪录放到缓存中
                _bus.RaiseEvent(new SetCacheEvent(usercoupon, key, key.CacheTime), cancellationToken);
            });
           await _bus.RemoveListCacheEvent<UserCoupon>(cancellationToken);

           await _bus.RemoveListCacheEvent<Coupon>(cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(DeleteUserCouponCommand request, CancellationToken cancellationToken)
    {
        var userCoupon = await _repository.GetByIdAsync(request.Id);
        if (userCoupon == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        await _repository.DeleteAsync(userCoupon);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<UserCoupon>.ByIdCacheKey.Create(userCoupon.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<UserCoupon>(cancellationToken);

           await _bus.RemoveListCacheEvent<Coupon>(cancellationToken);
        }

        return true;
    }
}