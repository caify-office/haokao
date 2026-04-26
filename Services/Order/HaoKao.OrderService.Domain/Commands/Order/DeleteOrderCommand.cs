namespace HaoKao.OrderService.Domain.Commands.Order;

/// <summary>
/// 删除订单表命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteOrderCommand(Guid Id) : Command("删除订单表");