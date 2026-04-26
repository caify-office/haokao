using HaoKao.OrderService.Domain.Commands.OrderSqInvoice;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.OrderInvoice;

[AutoMapTo(typeof(UpdateOrderInvoiceCommand))]
public class UpdateOrderInvoiceViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 申请状态
    /// </summary>
    [DisplayName("申请状态")]
    public RequestState RequestState { get; set; }

    /// <summary>
    /// 物流公司
    /// </summary>
    [DisplayName("物流公司")]
    public string LogisticsCompany { get; set; }

    /// <summary>
    /// 物流单号
    /// </summary>
    [DisplayName("物流单号")]
    public string ShippingNumber { get; set; }
}