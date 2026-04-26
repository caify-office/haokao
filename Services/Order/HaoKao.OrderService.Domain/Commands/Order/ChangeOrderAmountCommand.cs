using HaoKao.OrderService.Domain.Entities;

namespace HaoKao.OrderService.Domain.Commands.Order;

public record ChangeOrderAmountCommand(Guid OrderId, List<PurchaseProductContent> ProductContents) : Command("修改订单价格");