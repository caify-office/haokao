namespace HaoKao.OrderService.Application.PayHandler;

public interface IOrderReturn
{
    /// <summary>
    /// 当前通知回调签名
    /// </summary>
    string CurrentPayNotifySign { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    Guid OrderId { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    string OrderNumber { get; set; }
}