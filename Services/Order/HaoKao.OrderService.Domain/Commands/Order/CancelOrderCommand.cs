namespace HaoKao.OrderService.Domain.Commands.Order;

/// <summary>
/// 取消订单
/// </summary>
/// <param name="Id">主键</param>
public record CancelOrderCommand(Guid Id) : Command("取消订单");