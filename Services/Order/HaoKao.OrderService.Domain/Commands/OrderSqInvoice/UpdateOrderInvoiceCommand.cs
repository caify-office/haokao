using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Domain.Commands.OrderSqInvoice;

/// <summary>
/// 更新订单发票申请命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="RequestState">申请状态</param>
/// <param name="LogisticsCompany">物流公司</param>
/// <param name="ShippingNumber">物流单号</param>
public record UpdateOrderInvoiceCommand(
    Guid Id,
    RequestState RequestState,
    string LogisticsCompany,
    string ShippingNumber
) : Command("更新订单发票申请");