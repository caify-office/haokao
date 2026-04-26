using HaoKao.Common.Events.Coupon;
using HaoKao.Common.Events.StudentPermission;
using HaoKao.OrderService.Domain.Commands.Order;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Repositories;
using Newtonsoft.Json;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using HaoKao.Common.Enums;
using HaoKao.Common.Events.Student;
using HaoKao.OrderService.Domain.Entities;

namespace HaoKao.OrderService.Domain.CommandHandlers;

public class OrderCommandHandler(
    IUnitOfWork<Order> uow,
    IMediatorHandler bus,
    IOrderRepository repository,
    IEventBus eventBus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateOrderCommand, bool>,
    IRequestHandler<FinishPayOrderCommand, bool>,
    IRequestHandler<DeleteOrderCommand, bool>,
    IRequestHandler<ChangeOrderAmountCommand, bool>,
    IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = OrderFactory.CreateDefaultOrder(request, new DefaultDiscountCalculator());
        var sourceModel = 0;

        if (order.ActualAmount == 0)
        {
            order = OrderFactory.CreateZeroPurchaseOrder(request, new DefaultDiscountCalculator());
            sourceModel = 3; // 0元购
        }

        if (request.CreatorId != Guid.Empty)
        {
            order = OrderFactory.CreateBackgroundAddOrder(request, new DefaultDiscountCalculator());
            sourceModel = 2; // 后台添加
        }
        //重新计算周期产品过期时间
        order.PurchaseProductContents.ForEach(x =>
        {
            if (x.ExpiryTimeTypeEnum == (int)ExpiryTimeTypeEnum.Day)
            {
                x.ExpiryTime = DateTime.Now.AddDays(x.Days);
            }
        });

        await repository.AddAsync(order);

        if (await Commit())
        {
            await UpdateEntityCache(order, cancellationToken);
            await RemoveListCache(cancellationToken);

            // 0元购买的订单不走支付流程
            if (order.OrderState == OrderState.PaymentSuccessful)
            {
                PublishSuccessfullyPaidEventToProduct(order, sourceModel);
                PublishUpdateIsUseEvent(order);
                await repository.AddProductSales(order);
            }
        }

        return true;
    }

    public async Task<bool> Handle(FinishPayOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id);
        if (order == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应订单表的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        // 如果状态是支付成功，则直接返回
        if (order.OrderState == OrderState.PaymentSuccessful) return true;

        order.OrderState = request.OrderState;
        order.PlatformPayerId = request.PlatformPayerId;
        order.PlatformPayerName = request.PlatformPayerName;
        order.OrderNumber = request.OrderNumber;
        // order.OrderSerialNumber = request.OrderSerialNumber;
        order.IosRestorePurchase = request.IosRestorePurchase;
        order.UpdateTime = DateTime.Now;

        //重新计算周期产品过期时间
        order.PurchaseProductContents.ForEach(x =>
        {
            if (x.ExpiryTimeTypeEnum == (int)ExpiryTimeTypeEnum.Day)
            {
                x.ExpiryTime = DateTime.Now.AddDays(x.Days);
            }
        });

        if (repository.Update(order))
        {
            PublishSuccessfullyPaidEventToProduct(order);
            PublishUpdateIsUseEvent(order);
            PublishUpdateStudentPaidEvent(order);
            await repository.AddProductSales(order);

            await RemoveEntityCache(order.Id, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id);
        if (order == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应订单表的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        order.OrderState = OrderState.Cancel;

        if (await Commit())
        {
            await RemoveEntityCache(order.Id, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id);
        if (order == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应订单表的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(order);

        if (await Commit())
        {
            await RemoveEntityCache(order.Id, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(ChangeOrderAmountCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.OrderId.ToString(), "未找到对应订单表的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (order.OrderState == OrderState.PaymentSuccessful)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.OrderId.ToString(), "支付成功的订单，无法进行价格调整", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }

        var contents = new List<PurchaseProductContent>(order.PurchaseProductContents.Capacity);
        foreach (var content in order.PurchaseProductContents)
        {
            var actualAmount = request.ProductContents.FirstOrDefault(x => x.ContentId == content.ContentId)?.ActualAmount ?? content.ActualAmount;
            contents.Add(new PurchaseProductContent
            {
                ContentId = content.ContentId,
                ContentName = content.ContentName,
                ContentType = content.ContentType,
                ContentAmount = content.ContentAmount,
                DiscountAmount = content.DiscountAmount,
                CouponAmount = content.CouponAmount,
                ActualAmount = actualAmount,
                AppleInPurchaseProductId = content.AppleInPurchaseProductId,
                ExpiryTime = content.ExpiryTime,
                Coupons = content.Coupons
            });
        }

        order.PurchaseProductContents = contents;
        order.ActualAmount = order.PurchaseProductContents.Sum(x => x.ActualAmount);

        if (await Commit())
        {
            await UpdateEntityCache(order, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    #region 私有方法

    private Task UpdateEntityCache(Order entity, CancellationToken cancellationToken)
    {
        var key = GirvsEntityCacheDefaults<Order>.ByIdCacheKey.Create(entity.Id.ToString());
        return _bus.RaiseEvent(new SetCacheEvent(System.Text.Json.JsonSerializer.Serialize(entity, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        }), key, key.CacheTime), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        var key = GirvsEntityCacheDefaults<Order>.ByIdCacheKey.Create(id.ToString());
        return _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        var listKey = GirvsEntityCacheDefaults<Order>.TenantListCacheKey.Create();
        return _bus.RaiseEvent(new RemoveCacheListEvent(listKey), cancellationToken);
    }

    private void PublishSuccessfullyPaidEventToProduct(Order order, int sourceModel = 0)
    {
        //发送事件通知产品服务
        var productEvent = new CreateStudentPermissionEvent
        (
            order.CreatorName,
            order.CreatorId,
            order.OrderNumber,
            order.PurchaseProductId,
            order.PurchaseName,
            JsonConvert.SerializeObject(order.PurchaseProductContents),
            sourceModel
        );
        eventBus.PublishAsync(productEvent);
    }

    private void PublishUpdateIsUseEvent(Order order)
    {
        var updateOrderIdEvent = new UpdateIsUseEvent(
            order.Id,
            order.OrderSerialNumber,
            order.PurchaseName,
            order.PurchaseProductId,
            order.OrderAmount,
            order.ActualAmount,
            order.UpdateTime,
            JsonConvert.SerializeObject(order.PurchaseProductContents)
        );
        eventBus.PublishAsync(updateOrderIdEvent);
    }

    private void PublishUpdateStudentPaidEvent(Order order)
    {
        if (order.ActualAmount >= 200)
        {
            var paidEvent = new UpdateStudentPaidEvent(order.CreatorId);
            eventBus.PublishAsync(paidEvent);
        }
    }

    #endregion
}

public class OrderFactory
{
    public static Order CreateDefaultOrder(CreateOrderCommand request, IDiscountCalculator discountCalculator)
    {
        var order = new Order
        {
            Id = request.Id,
            OrderSerialNumber = request.OrderSerialNumber,
            PurchaseProductId = request.PurchaseProductId,
            PurchaseName = request.PurchaseName,
            PurchaseProductType = request.PurchaseProductType,
            OrderAmount = request.OrderAmount,
            ActualAmount = request.ActualAmount,
            Phone = request.Phone,
            LiveId = request.LiveId,
            OrderState = OrderState.Paying,
            PurchaseProductContents = request.ProductContents,
            IosRestorePurchase = false,
            ClientId = request.ClientId,
            ClientName = request.ClientName,
        };

        // 计算出优惠券减免的价格
        foreach (var content in order.PurchaseProductContents)
        {
            // 没有使用优惠券的情况下，不需要计算优惠券减免的价格
            content.CouponAmount = content.Coupons?.Count > 0 ? discountCalculator.CalculateDiscount(content) : 0;
        }

        return order;
    }

    public static Order CreateZeroPurchaseOrder(CreateOrderCommand request, IDiscountCalculator discountCalculator)
    {
        var order = CreateDefaultOrder(request, discountCalculator);
        order.OrderState = OrderState.PaymentSuccessful;
        return order;
    }

    public static Order CreateBackgroundAddOrder(CreateOrderCommand request, IDiscountCalculator discountCalculator)
    {
        var order = CreateDefaultOrder(request, discountCalculator);

        order.OrderState = OrderState.PaymentSuccessful;
        order.CreatorId = request.CreatorId;
        order.CreatorName = request.CreatorName;
        order.CreateTime = request.CreateTime.Value;
        order.UpdateTime = request.UpdateTime.Value;

        return order;
    }
}

public interface IDiscountCalculator
{
    decimal CalculateDiscount(PurchaseProductContent content);
}

public class DefaultDiscountCalculator : IDiscountCalculator
{
    public decimal CalculateDiscount(PurchaseProductContent content)
    {
        return content.DiscountAmount - content.ActualAmount;
    }
}