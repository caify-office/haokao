namespace HaoKao.OrderService.Application.ViewModels.Order;

public class CurrentPayNotifyViewModel
{
    /// <summary>
    /// 平台支付者ID
    /// </summary>
    [Required]
    public Guid PlatformPayerId { get; set; }

    /// <summary>
    /// 平台订单Id
    /// </summary>
    [Required]
    public Guid OrderId { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    [Required]
    public string OrderNumber { get; set; }

    /// <summary>
    /// 通知签名
    /// </summary>
    [Required]
    public string CurrentPayNotifySign { get; set; }

    /// <summary>
    /// 是否为恢复购买
    /// </summary>
    [Required]
    public bool IosRestorePurchase { get; set; } = false;
}