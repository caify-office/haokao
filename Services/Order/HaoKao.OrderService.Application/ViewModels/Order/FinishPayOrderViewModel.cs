using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.Order;

public class FinishPayOrderViewModel
{
    /// <summary>
    /// 使用的平台配置的支付者的Id
    /// </summary>
    [DisplayName("使用的平台配置的支付者的Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid PlatformPayerId { get; set; }

    /// <summary>
    /// 使用的平台配置的支付者的名称
    /// </summary>
    [DisplayName("使用的平台配置的支付者的名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string PlatformPayerName { get; set; }

    /// <summary>
    /// Id
    /// </summary>
    [DisplayName("Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }

    /// <summary>
    /// 订单流水号
    /// </summary>
    [DisplayName("订单流水号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    [DisplayName("订单号")]
    public string OrderNumber { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [DisplayName("状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public OrderState OrderState { get; set; }

    [DisplayName("苹果内购恢复购买")]
    public bool IosRestorePurchase { get; set; }
}