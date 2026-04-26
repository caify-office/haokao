using Girvs.Cache.Configuration;
using HaoKao.Common.Events.Coupon;
using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.Order;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.Services.Web;

/// <summary>
/// 订单服务--Web
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class OrderWebService(
    ILocker locker,
    IEventBus eventBus,
    IOrderService service
) : IOrderWebService
{
    /// <summary>
    /// 根据Id获取订单详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<BrowseOrderViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <summary>
    /// 我的订单
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<OrderQueryViewModel> Get([FromQuery] OrderQueryViewModel queryViewModel)
    {
        queryViewModel.CreatorId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var result = await service.Get(queryViewModel);
        foreach (var x in result.Result)
        {
            x.VaildTime = x.CreateTime.AddHours(2);
        }
        // 返回过滤失效的订单
        if (queryViewModel.OrderState != OrderState.PaymentSuccessful)
        {
            result.Result = result.Result.Where(x => x.VaildTime > DateTime.Now).ToList();
        }
        return result;
    }

    /// <summary>
    /// 是否付费用户
    /// </summary>
    [HttpGet]
    public async Task<bool> IsPaidUser()
    {
        var queryViewModel = new OrderQueryViewModel
        {
            CreatorId = EngineContext.Current.ClaimManager.GetUserId().ToGuid(),
            PageIndex = 1,
            PageSize = 1,
            IsPaidOrder = true,
            OrderState = OrderState.PaymentSuccessful
        };
        var resut = await Get(queryViewModel);
        return resut.Result.Any();
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> Create([FromBody] CreateOrderViewModel model)
    {
        var cacheKey = GirvsEntityCacheDefaults<Order>.ByIdCacheKey.Create($"{model.PurchaseName}{DateTime.Now.Ticks}");
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);
        var orderId = Guid.Empty;
        // 同时并发加锁机制
        await locker.PerformActionWithLockAsync(cacheKey.Key, timeSpan, async () =>
        {
            orderId = await service.Create(model);
            var couponIds = string.Join(",", model.PurchaseProductContents.Where(x => x.Coupons != null).SelectMany(x => x.Coupons.Select(c => c.Id)));
            await PublishUpdateOrderLocked(orderId, couponIds);
        });
        return orderId;
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("{id:guid}")]
    public async Task Cancel(Guid id)
    {
        // 取消订单
        await service.Cancel(id);

        // 取消订单解除锁定
        await PublishUpdateLocked(id);
    }

    /// <summary>
    /// 发布解锁事件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Task PublishUpdateLocked(Guid id)
    {
        var updateIsLockedEvent = new UpdateIsLockedEvent(id);
        return eventBus.PublishAsync(updateIsLockedEvent);
    }

    /// <summary>
    /// 创建订单更新优惠券订单id和锁定状态
    /// </summary>
    /// <param name="id">订单id</param>
    /// <param name="couponIds">优惠券id</param>
    /// <returns></returns>
    private Task PublishUpdateOrderLocked(Guid id, string couponIds)
    {
        var updateOrderIdEvent = new UpdateOrderIdEvent(couponIds, id);
        return eventBus.PublishAsync(updateOrderIdEvent);
    }
}