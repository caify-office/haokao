using HaoKao.OrderService.Domain.Entities;

namespace HaoKao.OrderService.Application.ViewModels.Order;

public class ChangeOrderAmountViewModel
{
    /// <summary>
    /// 订单Id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// 产品详细内容
    /// </summary>
    public List<PurchaseProductContent> ProductContents { get; set; } = [];
}