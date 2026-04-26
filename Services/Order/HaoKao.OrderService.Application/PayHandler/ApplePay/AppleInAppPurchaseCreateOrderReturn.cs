namespace HaoKao.OrderService.Application.PayHandler.ApplePay;

public class AppleInAppPurchaseCreateOrderReturn : IOrderReturn
{
    /// <summary>
    /// 订单号
    /// </summary>
    public Guid OrderId { get; set; }

    public string OrderNumber { get; set; }

    public string CurrentPayNotifySign { get; set; }
}