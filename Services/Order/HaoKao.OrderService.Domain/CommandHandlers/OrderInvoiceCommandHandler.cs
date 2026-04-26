using HaoKao.Common.Extensions;
using HaoKao.OrderService.Domain.Commands.OrderSqInvoice;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Repositories;

namespace HaoKao.OrderService.Domain.CommandHandlers;

public class OrderInvoiceCommandHandler(
    IUnitOfWork<OrderInvoice> uow,
    IMediatorHandler bus,
    IOrderInvoiceRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateOrderInvoiceCommand, bool>,
    IRequestHandler<UpdateOrderInvoiceCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateOrderInvoiceCommand request, CancellationToken cancellationToken)
    {
        var entity = request.MapToEntity<OrderInvoice>();

        await repository.AddAsync(entity);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<OrderInvoice>.ByIdCacheKey.Create(entity.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            await _bus.RemoveListCacheEvent<OrderInvoice>(cancellationToken);

            key = GirvsEntityCacheDefaults<Order>.ByIdCacheKey.Create(entity.OrderId.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            await _bus.RemoveListCacheEvent<Order>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateOrderInvoiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsync(x => x.Id == request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }

        entity.RequestState = request.RequestState;

        if (entity.RequestState == RequestState.Success && entity.InvoiceFormat == InvoiceFormat.Paper)
        {
            entity.LogisticsCompany = request.LogisticsCompany;
            entity.ShippingNumber = request.ShippingNumber;
            entity.ShippingTime = DateTime.Now;
        }

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<OrderInvoice>.ByIdCacheKey.Create(entity.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            await _bus.RemoveListCacheEvent<OrderInvoice>(cancellationToken);

            key = GirvsEntityCacheDefaults<Order>.ByIdCacheKey.Create(entity.OrderId.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            await _bus.RemoveListCacheEvent<Order>(cancellationToken);
        }

        return true;
    }
}