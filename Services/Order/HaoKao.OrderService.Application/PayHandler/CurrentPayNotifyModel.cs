using HaoKao.OrderService.Domain.Entities;

namespace HaoKao.OrderService.Application.PayHandler;

public class CurrentPayNotifyModel
{
    /// <summary>
    /// 平台订单Id
    /// </summary>
    public Order Order { get; set; }

    /// <summary>
    /// 订单流水号
    /// </summary>
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 通知签名
    /// </summary>
    public string CurrentPayNotifySign { get; set; }

    /// <summary>
    /// 是否为恢复购买
    /// </summary>
    public bool IosRestorePurchase { get; set; } = false;
}